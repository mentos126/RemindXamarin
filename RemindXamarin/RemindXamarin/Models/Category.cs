using System;

namespace RemindXamarin.Models
{
	public class Category
	{
		public int ID { get; set; }
        public String name { get; set; }
        public String icon { get; set; }
        public int color { get; set; }

        public Category(String name, String icon , int color) {
            DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            this.ID = (int)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds; this.name = name;
			this.icon = icon;
			this.color = color;
		}

		public String toString() {
			return " [ "
					+ "\n\t"+name
					+ "\n\t"+icon
					+ "\n\t"+color
					+ "\n] ";
		}

	}

}