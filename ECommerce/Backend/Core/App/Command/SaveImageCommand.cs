using App.Core.Interface;
using App.Core.Model;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.Command
{
    public class SaveImageCommand:IRequest<ResponceDto>
    {
        public IFormFile imageFile;
    }
    public class SaveImageCommandHandler : IRequestHandler<SaveImageCommand, ResponceDto>
    {
        private readonly IFileService _fileService;
        public SaveImageCommandHandler(IFileService fileService)
        {
            _fileService = fileService;
        }

        public async Task<ResponceDto> Handle(SaveImageCommand request, CancellationToken cancellationToken)
        {

            if(request.imageFile==null )
            {
                return new ResponceDto
                {
                    isSuccess = false,
                    message="no file choosen"
                };
            }

            var image = request.imageFile;
            var allowedFileExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            var filePath = await _fileService.SaveFileAsync(image, allowedFileExtensions);
            var fileUrl = $"https://localhost:7053/Uploads/{Path.GetFileName(filePath)}";

            return new ResponceDto
            {
                isSuccess=true,
                message = "image uploadede successfully",
                data=fileUrl
            };


            
        }
    }
}
