using System;

namespace RemindXamarin.Models
{
	public class Category
	{
		private int ID;
		private String name;
		private int icon;
		private int color;

		public Category(String name, int icon , int color) {
            DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            this.ID = (int)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds; this.setName(name);
			this.setIcon(icon);
			this.setColor(color);
		}

		public int getID() {return ID;}
		
		public String getName() {return name;}
		public void setName(String name) {this.name = name;}
		
		public int getIcon() {return icon;}
		public void setIcon(int icon) {this.icon = icon;}

		public int getColor() {return color;}
		public void setColor(int color) {this.color = color;}

		public String toString() {
			return " [ "
					+ "\n\t"+name
					+ "\n\t"+icon
					+ "\n\t"+color
					+ "\n] ";
		}

	}

}