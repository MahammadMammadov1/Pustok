using Pustok.Exceptions;
using Pustok.Models;
using Pustok.Repositories;
using Pustok.Repositories.Interfaces;


namespace Pustok.Services.Implementations
{
    public class SliderService : ISliderService
    {
        private readonly ISliderRepository _sliderRepository;

        public SliderService(ISliderRepository sliderRepository)
        {
            _sliderRepository = sliderRepository;
        }
        public async Task CreateAsync(Slider slider)
        {
            string fileName = "";
            if (slider.FormFile != null)
            {
                fileName = slider.FormFile.FileName;
                if (slider.FormFile.ContentType != "image/jpeg" && slider.FormFile.ContentType != "image/png")
                {
                    throw new TotalSliderExceptions("FormFile", "you can only add png or jpeg file");
                }

                if (slider.FormFile.Length > 1048576)
                {
                    throw new TotalSliderExceptions("FormFile", "file must be lower than 1 mb");
                }

                if (slider.FormFile.FileName.Length > 64)
                {
                    fileName = fileName.Substring(fileName.Length - 64, 64);
                }

                fileName = Guid.NewGuid().ToString() + fileName;

                string path = "C:\\Users\\II Novbe\\Desktop\\Pustok-Last-version\\Pustok\\wwwroot\\uploads\\sliders\\" + fileName;
                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    slider.FormFile.CopyTo(fileStream);
                }
                slider.ImageUrl = fileName;
            }
            else
            {
                throw new TotalSliderExceptions("FormFile", "image is required");
                
            }

            await _sliderRepository.Create(slider);
            await _sliderRepository.Save();
        }

        public async Task DeleteAsync(int id)
        {
            

            var wantedSlide = await _sliderRepository.GetById(id);

            if (wantedSlide == null) throw new NullReferenceException();

            _sliderRepository.Delete(wantedSlide);
            await _sliderRepository.Save();
        }

        public async Task<List<Slider>> GetAllAsync()
        {
            return await _sliderRepository.All();
        }

        public Task<Slider> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Slider slider)
        {
            Slider wanted = await _sliderRepository.GetById(slider.Id);

            if (wanted == null)
            {
                throw new NullReferenceException();
            }
            string oldFilePath = "C:\\Users\\II Novbe\\Desktop\\Pustok-Last-version\\Pustok\\wwwroot\\uploads\\sliders\\" + wanted.ImageUrl;

            if (slider.FormFile != null)
            {

                string newFileName = slider.FormFile.FileName;
                if (slider.FormFile.ContentType != "image/jpeg" && slider.FormFile.ContentType != "image/png")
                {
                    throw new TotalSliderExceptions("FormFile", "you can only add png or jpeg file");
                }

                if (slider.FormFile.Length > 1048576)
                {
                    throw new TotalSliderExceptions("FormFile", "file must be lower than 1 mb");
                }

                if (slider.FormFile.FileName.Length > 64)
                {
                    newFileName = newFileName.Substring(newFileName.Length - 64, 64);
                }

                newFileName = Guid.NewGuid().ToString() + newFileName;

                string newFilePath = "C:\\Users\\II Novbe\\Desktop\\Pustok-Last-version\\Pustok\\wwwroot\\uploads\\sliders\\" + newFileName;
                using (FileStream fileStream = new FileStream(newFilePath, FileMode.Create))
                {
                    slider.FormFile.CopyTo(fileStream);
                }

                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                }

                wanted.ImageUrl = newFileName;
            }

            wanted.Title = slider.Title;
            wanted.Description = slider.Description;
            wanted.RedirctText = slider.RedirctText;
            wanted.RedirctUrl = slider.RedirctUrl;

            await _sliderRepository.Save();
        }
    }
}
