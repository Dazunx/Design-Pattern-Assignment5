using System;
using System.Collections;

namespace assignment5
{

    public class WeatherStation
    {
        public static void Main(String[] args)
        {
            WeatherData weatherData = new WeatherData();

            CurrentConditionsDisplay currentDisplay = new CurrentConditionsDisplay(weatherData);
            StatisticsDisplay statisticsDisplay = new StatisticsDisplay(weatherData);
            ForecastDisplay forecastDisplay = new ForecastDisplay(weatherData);

            weatherData.setMeasurements(80, 65, 30.4f);
            weatherData.setMeasurements(82, 70, 29.2f);
            weatherData.setMeasurements(78, 90, 29.2f);
        }
    }
    public interface Subject
    {
        public void registerObserver(Observer o);
        public void removeObserver(Observer o);
        public void notifyObservers();
    }

    public interface Observer
    {
        public void update(float temp, float humidity, float pressure, float max, float min);
    }
    public interface DisplayElement
    {
        public void display();
    }


    public class WeatherData : Subject
    {
        private ArrayList observers;
        private float temperature;
        private float humidity;
        private float pressure;
        private float max;
        private float min;

        public WeatherData()
        {
            observers = new ArrayList();
        }

        public void registerObserver(Observer o)
        {
            observers.Add(o);
        }

        public void removeObserver(Observer o)
        {
            int i = observers.IndexOf(o);
            if (i >= 0)
            {
                observers.Remove(i);
            }
        }

        public void notifyObservers()
        {
            for (int i = 0; i < observers.Count; i++)
            {
                Observer observer = (Observer)observers[i];
                observer.update(temperature, humidity, pressure, max, min);
            }
        }

        public void measurementsChanged()
        {
            notifyObservers();
        }

        public void setMeasurements(float temperature, float humidity, float pressure)
        {
            this.temperature = temperature;
            this.humidity = humidity;
            this.pressure = pressure;

            if (max==0 && min==0) { max = temperature; min = temperature; }
            if (temperature > max) { max = temperature; }
            if (temperature < min) { min = temperature; }

            measurementsChanged();
        }
        // other WeatherData methods here
    }


    public class CurrentConditionsDisplay : Observer, DisplayElement 
    {
        private float temperature;
        private float humidity;
        private Subject weatherData;

        public CurrentConditionsDisplay(Subject weatherData)
        {
            this.weatherData = weatherData;
            weatherData.registerObserver(this);
        }

        public void update(float temperature, float humidity, float pressure, float max, float min)
        {
            this.temperature = temperature;
            this.humidity = humidity;
            display();
        }

        public void display()
        {
            Console.WriteLine("Current conditions: " + temperature + " degrees and " + humidity + "% humidity");
        }
    }

    public class StatisticsDisplay : Observer, DisplayElement
    {
        private float max;
        private float min;
        private float avg;
        private Subject weatherData;


        public StatisticsDisplay(Subject weatherData)
        {
            this.weatherData = weatherData;
            weatherData.registerObserver(this);
        }

        public void update(float temperature, float humidity, float pressure, float max, float min)
        {
            this.max = max;
            this.min = min;
            avg = (this.max + this.min) / 2;
            display();
        }

        public void display()
        {
            Console.WriteLine("Avg/Max/Min temperature: " + avg + "/" + max + "/" + min);
        }
    }

    public class ForecastDisplay : Observer, DisplayElement
    {
        private float temperature;
        private float humidity;
        private Subject weatherData;
        private string forecast;

        public ForecastDisplay(Subject weatherData)
        {
            this.weatherData = weatherData;
            weatherData.registerObserver(this);
        }

        public void update(float temperature, float humidity, float pressure, float max, float min)
        {
            this.temperature = temperature;
            this.humidity = humidity;
            if (humidity < 70) { this.forecast = "Improving weather on the way!"; }
            else if (humidity >= 70 && humidity < 90) { this.forecast = "Watch out for cooler, rainy weather"; }
            else { this.forecast = "More of the same"; }
            display();
        }

        public void display()
        {
            Console.WriteLine("Forecast: " + forecast);
        }
    }
}