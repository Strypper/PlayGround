using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayGround.Models
{
    public class ListModelTest
    {
        public string Title { get; set; }
        public string ImgURL { get; set; }
    }
    public class ItemManager
    {
        public static List<ListModelTest> GetT()
        {
            var data = new List<ListModelTest>();
            data.Add(new ListModelTest { Title = "1", ImgURL = "/Assets/TestPics/Idea 1.png" });
            data.Add(new ListModelTest { Title = "2", ImgURL = "/Assets/TestPics/Idea 2.png" });
            data.Add(new ListModelTest { Title = "3", ImgURL = "/Assets/TestPics/Idea 3.png" });
            data.Add(new ListModelTest { Title = "4", ImgURL = "/Assets/TestPics/Idea 4.png" });
            data.Add(new ListModelTest { Title = "5", ImgURL = "/Assets/TestPics/Idea 5.png" });
            data.Add(new ListModelTest { Title = "6", ImgURL = "/Assets/TestPics/Idea 6.png" });
            data.Add(new ListModelTest { Title = "7", ImgURL = "/Assets/TestPics/Idea 7.png" });
            data.Add(new ListModelTest { Title = "8", ImgURL = "/Assets/TestPics/Idea 8.png" });
            data.Add(new ListModelTest { Title = "9", ImgURL = "/Assets/TestPics/Idea 9.png" });
            data.Add(new ListModelTest { Title = "10", ImgURL = "/Assets/TestPics/Idea 10.png" });
            data.Add(new ListModelTest { Title = "11", ImgURL = "/Assets/TestPics/Idea 11.png" });
            data.Add(new ListModelTest { Title = "12", ImgURL = "/Assets/TestPics/Idea 12.png" });
            data.Add(new ListModelTest { Title = "13", ImgURL = "/Assets/TestPics/Idea 13.png" });
            data.Add(new ListModelTest { Title = "14", ImgURL = "/Assets/TestPics/Idea 14.png" });
            data.Add(new ListModelTest { Title = "15", ImgURL = "/Assets/TestPics/Idea 15.png" });
            data.Add(new ListModelTest { Title = "16", ImgURL = "/Assets/TestPics/Idea 16.png" });
            data.Add(new ListModelTest { Title = "17", ImgURL = "/Assets/TestPics/Idea 17.png" });
            return data;
        }
    }
}
