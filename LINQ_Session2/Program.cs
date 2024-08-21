using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Text.RegularExpressions;       
using static LINQ_Session2.ListGenerator;
namespace LINQ_Session2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Filteration [Restriction] (Where)

            //First overLoad of Where: 
            //Product out of stock
            //Fluent Syntax
            var Result = ListGenerator.ProductsList.Where(P => P.UnitsInStock == 0);

            //Query experssion:

            Result = from p in ListGenerator.ProductsList
                     where p.UnitsInStock == 0
                     select p;
            Console.WriteLine("============");

            Result = ProductsList.Where(P => P.UnitsInStock > 0 && P.Category == "Meat/Poultry");
            Result = from p in ProductsList
                     where p.UnitsInStock > 0 && p.Category == "Meat/Poultry"
                     select p;

            #endregion
            #region Second overload : Indexer Where

            //indexer where used with fluent syntax ONLY!
            Result = ProductsList.Where((p, i) => i < 10 && p.UnitsInStock == 0);
            Result = ProductsList.OfType<product02>();
            Console.WriteLine(" *****************");


            #endregion
            // ================= Ordering Operators ==================//
            #region Ordering Operators

            // Fluent Syntax

            Result = ProductsList.OrderByDescending(p => p.UnitsInStock);

            // Query syntax


            Result = from p in ProductsList
                     orderby p.UnitsInStock descending
                     select p;

            // Order with one more column:
            // Use ThenBy in Fluent

            Result = ProductsList.OrderBy(p => p.UnitsInStock).ThenBy(p => p.UnitPrice);

            // Order with one more column in Query syntax:
            Result = from p in ProductsList
                     orderby p.UnitsInStock, p.UnitPrice
                     select p;



            //Reverse 

            Result = ProductsList.Where(p => p.UnitsInStock == 0).Reverse();





            #endregion

            #region Transformation [Projection] Operators [Select , Select Many]

            // => For select for sequence and display it.
            // Fluent syntax

            var Result02 = ProductsList.Select(p => p.ProductName);

            Result02 = from p in ProductsList
                       select p.ProductName;

            //Select customer Name:
            Result02 = CustomersList.Select(C => C.CustomerName);

            Result02 = from C in CustomersList
                       select C.CustomerName;


            //============= Select Many=======//
            // use Select Many when select to array or collection
            var ResultArray = CustomersList.SelectMany(C => C.Orders);

            ResultArray = from c in CustomersList
                          from o in c.Orders
                          select o;

            // Select more than one => use ananoymous type
            // CLR will create class in RunTime and override to string
            var result = ProductsList.Select(p => new { p.ProductID, p.ProductName });

            result = from p in ProductsList
                     select new
                     {

                         ProductID = p.ProductID,
                         ProductName = p.ProductName,
                     };

            /// Select product in stock and apply discout 10% on its price
            var Result03 = ProductsList.Where(P => P.UnitsInStock > 0)
                                        .Select(P => new
                                        {
                                            Id = P.ProductID,
                                            Name = P.ProductName,
                                            OldPrice = P.UnitPrice,
                                            NewPrice = P.UnitPrice - (P.UnitPrice * 0.1m),
                                        });
            Result03 = from P in ProductsList
                       where P.UnitPrice > 0
                       select
                       new
                       {
                           Id = P.ProductID,
                           Name = P.ProductName,
                           OldPrice = P.UnitPrice,
                           NewPrice = P.UnitPrice - (P.UnitPrice * 0.1m),
                       };


            // Second OverLOad of Select : Index Select only vaild in Fluent Syntax

            var Result04 = ProductsList.Where(P => P.UnitsInStock > 0).Select((P, I) =>
             new
             {
                 Index = I,
                 Name = P.ProductName,
             }
             );


            #endregion

            #region Ordering Operators 
            //Get Products Ordered By Price Asc

            // 1. Fluent Syntax
            var Result05 = ProductsList.OrderBy(p => p.UnitPrice);


            // 2.Query list

            Result05 = from P in ProductsList
                       orderby P.UnitPrice
                       select P;
            //Get Products Ordered By Price Desc

            Result05 = ProductsList.OrderByDescending(p => p.UnitPrice);

            Result05 = from P in ProductsList
                       orderby P.UnitPrice descending
                       select P;


            //Get Products Ordered By Price Asc and Number Of Items In Stock

            Result05 = ProductsList.OrderBy(P => P.UnitPrice).ThenByDescending(P => P.UnitsInStock);

            var Result06 = ProductsList.Where(P => P.UnitsInStock == 0).Reverse();



            #endregion

            #region Elements Operator - Immediate Execution [Valid Only With Fluent Syntax]
            // Vaild with Fluent syntax Only:
            //First 

            // var Result07 = ProductsList.First(); // can return Null
            //Result07 = ProductsList.Last();
            // First and last may throw exception if the sequence is empty.
            List<Product> testProduct = new List<Product>();

            //Result07 = testProduct.FirstOrDefault();
            //Result07 = testProduct.LastOrDefault();
            //FirstOrDefault or LastOrDefault => if sequence is empty will return null [Default]
            //Result07 = ProductsList.First(P => P.UnitsInStock > 0);
            //Result07 = ProductsList.Last(P => P.UnitsInStock > 0);

            // first and last => if there is no matching element => throw exepction
            //Result07 = testProduct.FirstOrDefault(P => P.UnitsInStock > 0);
            //elementAt
            // Result07 = ProductsList.ElementAt(78);
            // Result07 = ProductsList.ElementAtOrDefault(78);

            //=======Single =====/

            //Result07 = ProductsList.Single(P => P.UnitsInStock ==0);


            //if sequence contain one elemnt match the function ===> Return it.
            // else will throw exception (sequnce isempty or more than one element match the function)
            //Console.WriteLine(Result07);

            // Result07 = ProductsList.SingleOrDefault(P => P.UnitsInStock == 0);


            //SingleOrDefault :
            //if sequence contain one elemnt match the function ===> Return it.
            //if sequence contain No elemnt match the function ===> Return Null.
            // else will throw exception more than one element match the function.

            //============= Query Syntax ============= //
            #region Query Syntax
            //Use hyprid syntax ===> Query experssion
            //  hyprid syntax  ==> (Query synta


            var Result08 = (from p in ProductsList
                            where p.UnitsInStock == 0
                            select
                            new
                            {
                                p.ProductID,
                                p.ProductName,
                                p.UnitsInStock
                            }).FirstOrDefault();



            #endregion



            //Console.WriteLine(Result08);


            //foreach (var product in Result07)
            //{
            //    Console.WriteLine($"{product} ");
            //}

            #endregion

            #region Aggregate Operators - Immediate Exection 

            /////========= COUNT =========//

            // var result009 = ProductsList.Count(); //LINQ operator
            //result009 = ProductsList.Count; //List property
            var result09 = ProductsList.Count(P => P.UnitsInStock == 0);

            Console.WriteLine(result09);

            /////========= maximum =========//

            var result10 = ProductsList.Max(); //Unhandled exception. System.ArgumentException: At least one object must implement IComparable.
                                               // Product must be implement interface Icomparable.
                                               // لازم نديه معلومة هنقارن بناء على ايه
            result10 = ProductsList.Min(); //Unhandled exception. System.ArgumentException: At least one object must implement IComparable.



            // Another overLoad for max and min

            //مش محتاجة هنا اعمل  implement for compare to beacuse i take it the condition he need to compare
            var MinLenght = ProductsList.Min(P => P.ProductName.Length);

            //Mix between query syntax and fluent syntax
            // ممكن اكتر من منتج طول اسمهم يساوي اقل طول لذلك يجب تحدديد انهي واحد فيهم عايزاه
            var result11 = (from p in ProductsList
                            where p.ProductName.Length == MinLenght
                            select p).FirstOrDefault();

            // Console.WriteLine($"MinLenght is : {MinLenght} and the product is : {result11}");

            var result12 = ProductsList.Sum(p => p.UnitPrice);
            result12 = ProductsList.Average(p => p.UnitPrice);

            string[] Names = { "AYA", "oMAR", "SOSO", "NAYA" };
            // do aggregation fot this string
            //Aggregate is used alot with API

            string result13 = Names.Aggregate((str01, str02) => $"{str01}  {str02}");


            //Console.WriteLine(result13);

            #endregion

            #region Casting [conversion] Operator (immediate)

            // To list 

            //List <Product> ProductList = ProductsList.Where(P => P.UnitsInStock == 0).ToList();
            //Product[] Result14 = ProductsList.Where(P => P.UnitsInStock == 0).ToArray();
            //Dictionary<long, Product> Result14 = ProductsList.Where(p => p.UnitsInStock == 0).ToDictionary(p => p.ProductID);

            // Dictionary <long , string > Result15 = ProductsList.Where(p=>p.UnitsInStock==0).ToDictionary (p => p.ProductID , p=>p.ProductName);
            //foreach (var product in Result15) Console.WriteLine($"KEY :{product.Key} , value = {product.Value}");
            HashSet<Product> Result15 = ProductsList.Where(p => p.UnitsInStock == 0).ToHashSet();
            // HashSet have key and its value is null
            // foreach (var product in Result15) Console.WriteLine(product);


            ArrayList Result16 = new ArrayList()

            {
                "Omar",
                    "Bayan" ,
                    1 ,
                    23
            };
            Console.WriteLine("===========");

            ///////// ============== OfType ===================//
            /// use to define 

            var result17 = Result16.OfType<string>();

            foreach (var product in result17)
                Console.WriteLine(product);

            #endregion

            #region Generation operators - deferred Execution
            /*
              two primary ways to invoke LINQ methods: through static method calls and extension method calls.

            Static Method Call
            In this approach, you call LINQ methods directly on the Enumerable or Queryable class as static methods.
            Static Method Call
                    In this approach, you call LINQ methods directly on the Enumerable or Queryable class as static methods.
            */
            // Valid only with fluent syntax only
            // the only way to call Generation operators  is as static methods from class enumbreable


            ////====== Range ======//
            // var result19 = Enumerable.Range(900, 10);

            ////====== Repeat ======//
            //result19 = Enumerable.Repeat(2, 100);
            var result20 = Enumerable.Repeat("Bayan", 100);
            var result19 = Enumerable.Repeat(new Product(), 100);
            // Returm ienumbreable 100 element each one is 2.


            ////====== Empty ======//

            var arrayProduct = Enumerable.Empty<Product>().ToArray();
            var list = Enumerable.Empty<Product>().ToList();
            List<Product> Products = new List<Product>();
            //both of them will generate an empty list of product

            //foreach (var num in result19)
            //{
            //    Console.Write(num + " ");
            //}

            #endregion

            #region Set Operators [Union family] - deferred execution
            //union 
            //work with two sequence with same dataType.
            var seq01 = Enumerable.Range(0, 100);
            var seq02 = Enumerable.Range(50, 100);


            //1.union : 
            // object member method (calling) // without dupliaction
            var result0 = seq01.Union(seq02);

            //////======== Concat : with duplication : ==========////
            result0 = seq01.Concat(seq02);

            ///////////============== Distinct : Remove the duplication =============//
            ///
            result0 = result0.Distinct();

            ////////===============Intersect===============//
            result0 = seq01.Intersect(seq02);

            /////////////================== execpt ==========///
            result0 = seq01.Except(seq02);

            Console.WriteLine("\n ================seq01 =======");

            foreach (var product in seq01)
            {
                Console.Write(product + " ");
            }

            Console.WriteLine("\n ================result0 =======");
            foreach (var product in result0)
            {
                Console.Write(product + " ");
            }

            #endregion
            #region Quntifier Operators - Deffered  Execution - Return Boolen
            Console.WriteLine("\n***********************************************************");
            //check if the element have at least one element so it return true , if not resturn false
            bool answer = ProductsList.Any();



            //check if the element have at least one element contains the conditions so it return true , if not resturn false

            //answer = ProductsList.Any( p=> p .UnitsInStock ==0);

            /////======== All elemet match the condition ===================///
            answer = ProductsList.All(P => P.UnitsInStock == 0);
            answer = seq01.SequenceEqual(seq02);

            Console.WriteLine(answer);

            #endregion

            #region Zipping operator -ZIP
            string[] names = { "Omar", "Amr", "aHMED", "Aya", "Bayan" };
            int[] numbers = Enumerable.Range(1, 10).ToArray();
            char[] chars = { 'a', 'n', };

            //ZIP : اضغطهم 

            /*  var result00 = names.Zip(numbers);*/ /*(Omar, 1)
                                                  (Amr, 2)
                                                  (aHMED, 3)
                                                  (Aya, 4)
                                                  (Bayan, 5) */
            //var result000 = names.Zip(numbers, (name, number) => new
            //{
            //    index = number,
            //    name = name,
            //}
            //);

            //اللي يرجع على حسب اصغر سيكونس بينهم
            var result000 = names.Zip(numbers, chars);

            foreach (var n in result000)
            {
                Console.WriteLine(n);
            }
            /*
             * (Omar, 1, a)
               (Amr, 2, n)
             */

            #endregion
            #region Grouping operators
            //Get Products Grouped by Category
            // *1.Query syntax
            //var result23 = from p in ProductsList
            //               group p by p.Category;

            //**Fluent sytanx****//
            // var result23 = ProductsList.GroupBy(p => p.Category);

            //Get Products in Stock Grouped by Category

            // var result23 = ProductsList.Where(p => p.UnitsInStock == 0)
            //.GroupBy(p => p.Category);

            //result23 = from p in ProductsList
            //where p.UnitsInStock > 0
            //group p by p.Category;

            //Get Products in Stock Grouped by
            //Category That Contains More Than 10 Product

            var result23 = from p in ProductsList
                           where p.UnitsInStock > 0
                           group p by p.Category
                          into category
                           where category.Count() > 10
                           select category; // query syntax should finish with select or group by

            result23 = ProductsList.Where(p => p.UnitPrice > 0)
                       .GroupBy(p => p.Category)
                       .Where(p => p.Count() > 10);

            ////Get Category Name of Products in Stock That Contains More Than 10 Product and
            ///Number of Product In Each Category

            var result24 = ProductsList.Where(p => p.UnitsInStock > 0)
                           .GroupBy(p => p.Category)
                           .Where(c => c.Count() > 10)
                           .Select(X => new
                           {
                               CategoryName = X.Key,
                               count = X.Count()
                           });

            result24 = from p in ProductsList
                       where p.UnitsInStock > 0
                       group p by p.Category
                       into category //محتاجة امسك السكونس اللي فوق
                       where category.Count() > 10
                       select new
                       {
                           CategoryName = category.Key,
                           count = category.Count()
                       };


            foreach (var n in result24)
            {
                Console.WriteLine(n);
            }

            //foreach (var category in result23)
            //{
            //    Console.Write(category.Key + "*****\n" );
            //    foreach(var product in category)
            //    {
            //        Console.WriteLine("       " +product.ProductName);
            //    }


            //}
            //foreach(var n in result23)
            //{
            //    Console.WriteLine(n);
            //}



            #endregion
            #region Partioning Operators
            // in api we do pagenation 
            // divide the elements to pages
            // they use partion operators
            //////////// Take and TakeLast //////////////
            Console.WriteLine("\n =================");
            var partition = ProductsList.Take(10);
            partition = ProductsList.Where(p => p.UnitsInStock > 0).Take(5); // the first (5)
            partition = ProductsList.Where(p => p.UnitsInStock > 0).Take(5);
            partition = ProductsList.TakeLast(10);

            //////////// Skip and SkipLast //////////////

            partition = ProductsList.Skip(60);
            partition = ProductsList.Where(p => p.UnitsInStock > 0).Skip(2);



            partition = ProductsList.Take(10); //page 1
            partition = ProductsList.Skip(10).Take(10); //page 2

            int[] numbers01 = { 1, 2, 43, 5, 6, 70, 3 };
            var numbers01Result = numbers01.TakeWhile(num => num< 9); // when the condition occur and then return false the check for the rest of number stop.
                                                                      // 
                    ////////// Indexed TakeWhile
            numbers01Result = numbers01.TakeWhile((num , I) => num > I);
            numbers01Result = numbers01.SkipWhile(num => num % 3 != 0); //6 70 3



            #region Let and into [Valid only with Query syntax ]
            string[] names02 = { "Omar","ayan", "Ahemd", "fghhBo", "Sara" };
            // remove vowel letters from this array

            //using system.text.RegularExoerssions => that to use class (regex)

            var names02Result = from N in names02
                                select Regex.Replace(N, "[AOUIEaouie]", string.Empty)
                                into newNames //into :  you reset the sequance[the ouery] and you can use where again
                                where newNames.Length > 3
                                select newNames;



            /////////////// Let : continue Query with adding new Range variable :newNames
             names02Result = from N in names02
                                let NoVowelName= Regex.Replace(N, "[AOUIEaouie]", string.Empty)
                                where NoVowelName.Length > 3
                                select NoVowelName;

            foreach (var name in names02Result)
            {
                Console.WriteLine(name);
            }

            #endregion
            Console.WriteLine("");

            foreach (var p in numbers01Result)
            {
                Console.Write(p + " ");
            }









            #endregion







        }
    }
}
