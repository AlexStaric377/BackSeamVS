
//namespace Test
//{
//	// Полиморфизм и переопределяемость классов 
//	//
	
//	class Program
//	{
//		static void Main (string[] args )
//		{
//			//Car car = new Car();
//			Person person = new Person();
//			//person.Drive(car);
//			person.Drive(new Car());
//			person.Drive(new Sportcar());
//		}
//	}
	
	
//	public class Person
//	{
//		public void Drive(Car car)
//		{
//			car.Drive();
//		}
//	}
	
//	class Car 
//	{
//		protected virtual void StartEng()
//		{
//			Console.WriteLine("Старт");
			
//		}
		
//		public virtual void Drive()
//		{
			
//			StartEng();
//			Console.WriteLine("Я еду");
		
//		}
//	}
	
//	class Sportcar : Car
//	{
//		protected override void StartEng()
//		{
//			Console.WriteLine("Запуск");
			
//		}
		
//		public override void Drive()
		
//		{
//			StartEng();
//			Console.WriteLine("Я еду очень быстро");
//		}
		
//	}
	
//	//
//	// Абстрактные классы и методы
	
//	class ProgramAbs
//	{
//		static void Main (string[] args )
//		{
//			Player player = new Player();
			
//			Weapon[] inventary = { new Gun, new LaserGun, new Bow };
//			foreach( var item in inventary)
//			{
//				player.CheckInfo(item);
//				player.Fire(item);
				
//			}
			
//			//Gun gun = new Gun();
//			//player.Fire(gun);
//		}
//	}
//	// абстрактный класс
//	abstract class Weapon
//	{
//		// абстрактное свойство
//		public abstract int Dange {get; set;}
		
//		// абстрактный метод
//		public abstract void Fire();
		
//		public void ShowInfo()
//		{
//			Console.WriteLine($"{GetType().Name} Dange: {Dange}");
//		}
//	}
	
//	class Gun : Weapon
//	{
//		public override int Dange {get {return 5}; };
		
//		public override void Fire()
//		{
//			Console.WriteLine("Бах");
			
//		}
		
//	}
	
//	class LaserGun : Weapon
//	{
//		public override int Dange =>3 };
		
//		public override void Fire()
//		{
//			Console.WriteLine("Пиу");
			
//		}
		
//	}
	
//	class Bow : Weapon
//	{
//		public override int Dange {get {return 8}; };
//		public override void Fire()
//		{
//			Console.WriteLine("Чмок");
			
//		}
		
//	}
	
//	class Player
//	{
		
//		public void Fire(Weapon weapon)
//		{
//			weapon.Fire();
			
//		}
//		public void CheckInfo(Weapon weapon)
//		{
//			weapon.ShowInfo();
			
//		}
//	}
	
//}