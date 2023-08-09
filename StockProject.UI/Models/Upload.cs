namespace StockProject.UI.Models
{
    public class Upload
    {
        public static string ImageUpload(List<IFormFile> files, IWebHostEnvironment env, out bool result)
        {
            result = false;
            var uploads = Path.Combine(env.WebRootPath, "Uploads");

            foreach (var file in files)
            {
                if (file.ContentType.Contains("image"))// yüklenen dosya tipinin image uzantılarından olmasını bekliyoruz.
                {
                    //Eğerki content type'ı image/jpg image/png image/tiff

                    if (file.Length <= 4194304)
                    {
                        // 96549577 - C8F7 - 47D1 - 9BA1 - AAF5257D2FC7 =>Guid
                        // 96549577_c8F7_47d1_9Ba1_aaf5257d2Fc7 =>Guid
                        string uniqueName = $"{Guid.NewGuid().ToString().Replace("-", "_").ToLower()}.{file.ContentType.Split('/')[1]}";
                        //Split sonrası / karakterinden bölerek bize bir array verir. Oluşan array => {"image","jpeg"} gibidir.Biz uzantıyı almak için 1. Indexteki değeri kullanırız.

                        // ~/Uploads/96549577_c8F7_47d1_9Ba1_aaf5257d2Fc7.jpg
                        var filePath =Path.Combine(uploads, uniqueName);

                        using (var fileStream=new FileStream(filePath,FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                            result = true;
                            return filePath.Substring(filePath.IndexOf("\\Uploads\\"));
                        }
                    }
                    else
                    {
                        return "Resim boyutu 4 MB'den büyük olamaz!";
                    }
                }
                else
                {
                    return "Resim formatında bir dosya seçiniz!";
                }
            }
            return "Dosya seçilmedi";
        }
    }
}
