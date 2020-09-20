using System;
using System.Collections.Generic;

namespace MDSHO.Models
{
    [Serializable]
    public class WindowItem
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Title { get; set; }
        public int WindowBorderColor { get; set; }
        public int TitleBackgroundColor { get; set; }
        public double TitleBackgroundOpacity { get; set; }
        public int TitleTextColor { get; set; }
        public int WindowBackgroundColor { get; set; }
        public double WindowBackgroundOpacity { get; set; }
        public int WindowTextColor { get; set; }
        public List<WindowClone> WindowClones { get; set; }
        public List<Item> Items { get; set; }

        public WindowItem(int left,
                          int top,
                          int width,
                          int height,
                          string title,
                          int windowBorderColor,
                          int titleBackgroundColor,
                          double titleBackgroundOpacity,
                          int titleTextColor,
                          int windowBackgroundColor,
                          double windowBackgroundOpacity,
                          int windowTextColor)
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
            Title = title;
            WindowBorderColor = windowBorderColor;
            TitleBackgroundColor = titleBackgroundColor;
            TitleBackgroundOpacity = titleBackgroundOpacity;
            TitleTextColor = titleTextColor;
            WindowBackgroundColor = windowBackgroundColor;
            WindowBackgroundOpacity = windowBackgroundOpacity;
            WindowTextColor = windowTextColor;
            // Set the default values
            WindowClones = new List<WindowClone>();
            Items = new List<Item>();
        }
    }

}
