using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates_Events_Exercise04
{
    internal class Program
    {
        static void Main(string[] args)
        {

            NewsPublisher publisher = new NewsPublisher();
            Subscriber subscriber1 = new Subscriber("subscriber 1");
            Subscriber subscriber2 = new Subscriber("subscriber 2");



            // subscribtion
            subscriber1.Subscribe(publisher);
            subscriber2.Subscribe(publisher);

            // news publishing
            NewArticleEventArgs newNews1 = new NewArticleEventArgs("The syrian sanctions","Tramp decided to raise the sanction on syria");
            publisher.publishNews(newNews1);

            // unsubscribe
            subscriber1.UnSubscribe(publisher);
            // news publishing
            NewArticleEventArgs newNews2 = new NewArticleEventArgs("The black friday", "You don't want to avoid the perfect matching discount will happen on black friday");
            publisher.publishNews(newNews2);

        }
    }

    public class NewArticleEventArgs : EventArgs
    {
        public string title { get; set; }
        public string description { get; set; }
        public NewArticleEventArgs(string title , string description) {
            this.title = title;
            this.description = description;
        }

    }

    public class NewsPublisher
    {
        public EventHandler<NewArticleEventArgs> NewsPublished;
        public void publishNews(NewArticleEventArgs e)
        {
            NewsPublished?.Invoke(this, e);
        }
    }
    public class Subscriber
    {
        string Name { get; set; }
        public Subscriber(string name)
        {
            this.Name = name;
        }
        public void Subscribe(NewsPublisher newPublisher)
        {
            newPublisher.NewsPublished += ShowNews;
        }
        public void UnSubscribe(NewsPublisher newPublisher)
        {
            newPublisher.NewsPublished -= ShowNews;
        }
        public void ShowNews(object sender, NewArticleEventArgs e)
        {
            Console.WriteLine($"the {this.Name} recevied the news.....");
            Console.WriteLine($"the news titled {e.title}.....");
            Console.WriteLine($"the news is {e.description}.....");
            Console.WriteLine($"................................");
        }

    }
}
