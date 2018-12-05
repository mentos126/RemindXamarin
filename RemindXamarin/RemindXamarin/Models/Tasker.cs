using System;
using System.Collections;
using System.Collections.Generic;

namespace RemindXamarin.Models
{
    public class Tasker
    {
        private ArrayList listCategories = new ArrayList();
        private ArrayList listTasks = new ArrayList();
        private ArrayList listSportTasks = new ArrayList();

        public static String CATEGORY_NONE_TAG = "Aucune";
        public static String CATEGORY_SPORT_TAG = "Sport";
        private static Tasker INSTANCE = null;
        private static readonly object padlock = new object();
        public static Tasker Instance
        {
            get
            {
                lock (padlock)
                {
                    if (INSTANCE==null)
                    {
                        INSTANCE = new Tasker();
                    }
                    return INSTANCE;
                }
            }
        }

        public Tasker() {
            unserializeLists();

            if (getCategoryByName(CATEGORY_NONE_TAG) == null){
                addCategory( new Category(CATEGORY_NONE_TAG, 12, 0));
            }
            if (getCategoryByName(CATEGORY_SPORT_TAG) == null){
                addCategory( new Category(CATEGORY_SPORT_TAG, 13, 0));
            }
            serializeLists();
        }

        public ArrayList getListTasks() {return listTasks;}
        public void setListTasks(ArrayList listTasks) {this.listTasks = listTasks;}
        public void removeTask(Tache t) {listTasks.Remove(t);}
        public void removeTaskByID(int id){
            int temp =-1;
            for (int i =0; i < listTasks.Count; i++){
                Tache t = (Tache)listTasks[i];
                if ( t.getID() == id){
                    temp = i;
                    break;
                }
            }
            if (temp != -1){
                listTasks.Remove(temp);
            }
        }

        public Boolean addTask(Tache t) {

            if (t.GetType() == typeof(SportTask)) {
                return addSportTask((SportTask) t);
            }

            foreach(Tache x in listTasks) {
                if(x.toString().Equals(t.toString())) {
                    return false;
                }
            }
            listTasks.Add(t);
            //ReminderWorker.scheduleWorker(t);
            return true;
        }

        public ArrayList getListSportTasks() {return listSportTasks;}
        public void setListSportTasks(ArrayList listSportTasks) {this.listSportTasks = listSportTasks;}
        public void removeSportTask(SportTask t) {listSportTasks.Remove(t);}
        public void removeSportTaskByID(int id){
            int temp =-1;
            for (int i =0; i < listSportTasks.Count; i++){
                SportTask t = (SportTask)listSportTasks[i];
                if (t.getID() == id){
                    temp = i;
                    break;
                }
            }
            if (temp != -1){
                listSportTasks.Remove(temp);
            }
        }

        public Boolean addSportTask(SportTask t) {
            foreach(Tache x in listSportTasks) {
                if(x.toString().Equals(t.toString())) {
                    return false;
                }
            }
            listSportTasks.Add(t);
            return true;
        }

        public void editCategoryById(int id, Category c){
            Category cat = getCategoryByID(id);
            cat.setColor(c.getColor());
            cat.setIcon(c.getIcon());
            cat.setName(c.getName());
        }

        public ArrayList getListCategories() {return listCategories;}
        public void setListCategories(ArrayList listCategories) {this.listCategories = listCategories;}
        public void removeCategory(Category c) {listCategories.Remove(c);}
        public Boolean addCategory(Category c) {
            foreach(Category x in listCategories) {
                if(x.toString().Equals(c.toString())) {
                    return false;
                }
            }
            listCategories.Add(c);
            return true;
        }

        public void changeWithSaveIsActivatedNotification(Tache t) {
            t.setIsActivatedNotification(!t.getIsActivatedNotification());
            serializeLists();
        }

        public void serializeLists() {
            /*serializeList(listCategories, "Category.txt");
            serializeList(listTasks, "Tache.txt");
            serializeList(listSportTasks, "SportTask.txt");*/

        }	
        
        public void serializeList(ArrayList list, String name) {
            /*try {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(name, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, list);
                stream.Close();
            } catch (IOException e) {
                e.printStackTrace();
            }*/
        }
        
        public void unserializeLists() {
            unserializeListCategories();
            unserializeListTasks();
            unserializeListSportTasks();
        }
        
        public void unserializeListCategories() {
            /*ArrayList<Category> list = new ArrayList<Category>();
            try {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream("Category.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
                list = (MyObject) formatter.Deserialize(stream);
                stream.Close();
            }catch (Exception e){
                serializeLists();
                try{
                    IFormatter formatter = new BinaryFormatter();
                    Stream stream = new FileStream("Category.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
                    list = (MyObject) formatter.Deserialize(stream);
                    stream.Close();
                }catch (Exception e2){
                    e2.printStackTrace();
                }
                e.printStackTrace();
            }
            listCategories  = list;*/
        }
        
        public void unserializeListTasks() {
            /*ArrayList<Tache> list = new ArrayList<Tache>();
            try {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream("Tache.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
                list = (MyObject) formatter.Deserialize(stream);
                stream.Close();
            }catch (Exception e){
                serializeLists();
                try{
                    IFormatter formatter = new BinaryFormatter();
                    Stream stream = new FileStream("Tache.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
                    list = (MyObject) formatter.Deserialize(stream);
                    stream.Close();
                }catch (Exception e2){
                    e2.printStackTrace();
                }
                e.printStackTrace();
            }
            listTasks = list;*/
        }

        public void unserializeListSportTasks() {
            /*ArrayList<SportTask> list = new ArrayList<>();
            try {
            	IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream("SportTask.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
                list = (MyObject) formatter.Deserialize(stream);
                stream.Close();
            }catch (Exception e){
            	serializeLists();
            	try{
            		IFormatter formatter = new BinaryFormatter();
                    Stream stream = new FileStream("SportTask.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
                    list = (MyObject) formatter.Deserialize(stream);
                    stream.Close();
            	}catch (Exception e2){
            		e2.printStackTrace();
            	}
            	e.printStackTrace();
            }
            listSportTasks = list;*/
        }
        
        public void garbageCollectOld() {
            unserializeLists();
            ArrayList deletes = new ArrayList();
            DateTime now = new DateTime();
            for(int i=0; i < listTasks.Count; i++){
                Tache t = (Tache)listTasks[i];
                if (t.getDateDeb() != null && t.getNextDate().CompareTo(now) < 0) {
                    Tache t2 = (Tache) listTasks[i];
                    deletes.Add(t2.getID());
                }
            }
            foreach(int i in deletes) {
                removeTaskByID(i);
            }
            serializeLists();
        }

        public void sort(Boolean growing) {
            unserializeLists();
            int i, j;
            int N = listTasks.Count;
            for (j = N - 1; j > 0; j--)
            {
                for (i = 0; i < j; i++)
                {
                    Tache t1 = (Tache)listTasks[i];
                    Tache t2 = (Tache)listTasks[i +  1];
                    if (growing)
                    {
                        if (t1.getNextDate() > t2.getNextDate()) {
                            Tache tmp = (Tache)listTasks[i];
                            listTasks[i] = listTasks[i + 1];
                            listTasks[i + 1] = tmp;

                        }
                    }
                    else {
                        if (t1.getNextDate() < t2.getNextDate())
                        {
                            Tache tmp = (Tache)listTasks[i];
                            listTasks[i] = listTasks[i + 1];
                            listTasks[i + 1] = tmp;

                        }
                    }
                }
            }
            serializeLists();
        }

        public void sportSort(Boolean growing) {
            unserializeLists();
            int i, j;
            int N = listSportTasks.Count;
            for (j = N - 1; j > 0; j--)
            {
                for (i = 0; i < j; i++)
                {
                    Tache t1 = (Tache)listSportTasks[i];
                    Tache t2 = (Tache)listSportTasks[i + 1];
                    if (growing)
                    {
                        if (t1.getNextDate() > t2.getNextDate())
                        {
                            Tache tmp = (Tache)listSportTasks[i];
                            listSportTasks[i] = listSportTasks[i + 1];
                            listSportTasks[i + 1] = tmp;

                        }
                    }
                    else
                    {
                        if (t1.getNextDate() < t2.getNextDate())
                        {
                            Tache tmp = (Tache)listSportTasks[i];
                            listSportTasks[i] = listSportTasks[i + 1];
                            listSportTasks[i + 1] = tmp;

                        }
                    }
                }
            }
            serializeLists();
        }

        public ArrayList filter(String seq, Boolean growing) {
            ArrayList res = new ArrayList();
            sort(growing);
            seq = seq.ToUpper();
            foreach(Tache t in listTasks){
                String dateFormated = t.getNextDate().ToString("d MMMM yyyy"); 
                if(t.getName().ToUpper().Contains(seq)) {
                    res.Add(t);
                }else if(t.getCategory().getName().ToUpper().Contains(seq)){
                    res.Add(t);
                }else if(t.getDescription().ToUpper().Contains(seq)){
                    res.Add(t);
                }else if(dateFormated.ToUpper().Contains(seq)){
                    res.Add(t);
                }
            }
            return res;
        }

        public ArrayList sportFilter(String seq, Boolean growing) {
            ArrayList res = new ArrayList();
            sort(growing);
            seq = seq.ToUpper();
            foreach(SportTask t in listSportTasks){
                String dateFormated = t.getNextDate().ToString("d MMMM yyyy");
                if (t.getName().ToUpper().Contains(seq)) {
                    res.Add(t);
                }else if(t.getCategory().getName().ToUpper().Contains(seq)){
                    res.Add(t);
                }else if(t.getDescription().ToUpper().Contains(seq)){
                    res.Add(t);
                }else if(dateFormated.ToUpper().Contains(seq)){
                    res.Add(t);
                }
            }
            return res;
        }

        public Tache getTaskByID(int id){
            foreach(Tache t in listTasks){
                if( t.getID() ==  id) {
                    return t;
                }
            }
            return null;
        }

        public SportTask getSportTaskByID(int id){
            foreach(SportTask t in listSportTasks){
                if( t.getID() ==  id) {
                    return t;
                }
            }
            return null;
        }

        public Category getCategoryByID(int id){
            foreach(Category c in listCategories){
                if( c.getID() ==  id) {
                    return c;
                }
            }
            return null;
        }

        public Category getCategoryByName(String catName){
            foreach (Category c in listCategories){
                if (c.getName().Equals(catName)){
                    return c;
                }
            }
            return null;
        }

        public ArrayList getTasksByCategory(Category c){
            ArrayList matches = new ArrayList();
            foreach (Tache t in getListTasks()){
                if (t.getCategory().Equals(c)){
                    matches.Add(t);
                }
            }
            return matches;
        }

    }
}