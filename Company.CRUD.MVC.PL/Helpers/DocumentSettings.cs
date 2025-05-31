namespace Company.CRUD.MVC.PL.Helpers
{
    public class DocumentSettings
    {
        // 1. Upload
        public static string UploadFile(IFormFile file, string folderName)  
        {
            // 1. Get Location Folder Path


            //static Path
            //string folderPath = $"D:\\Route Academy\\07 ASP.NET Core MVC\\Session 06\\Assignment\\Session06MVCDemo&Task.Route Solution\\Session04MVCDemo&Task.Route.PL\\wwwroot\\files\\{folderName}";  
           
            //dynamic path
            //string folderPath = Directory.GetCurrentDirectory() + @"wwwroot\files" + folderName;
            string folderPath = Path.Combine(Directory.GetCurrentDirectory() , @"wwwroot\files" , folderName);


            // 2. Get File Name Make it Unique

            string fileName = $"{Guid.NewGuid()}{file.FileName}";

            // 3. Get File Path --> folderPath + fileName

            string filePath = Path.Combine(folderPath, fileName);

            // 4. Save File as Stream : Data Per Time 

            using var fileStream = new FileStream(filePath,FileMode.Create); 

            file.CopyTo(fileStream);

            return fileName ;

        }


		// 2. Delete

		public static void DeleteFile(string fileName,string folderName)
        {
            //string folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files" ,folderName); 

            //string filePath = Path.Combine(folderPath, fileName);
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName, fileName);

            if(File.Exists(filePath))
            {
                File.Delete(filePath);
            }

        }


    }
}
