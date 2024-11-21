
using System;
using System.Linq.Expressions;
using System.Timers;
using static Prison.Program;

namespace Prison
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int verifiedInput; // initialize input variable
            Boolean loggedIn = false; // initialize loggedIn to false

            Prison capeCounty = new Prison(); // create a new prison object
            Guard newGuard = capeCounty.hireGuard(); // create a new guard object by calling the prison class's hireguard function

            
            for(int i =0; i < 5; i++) // run 5 times to put some starters prisoners into our prison
            {
                capeCounty.simpleAddPrisoner();  // call the prison class's function simpleAddPrisoner to create a new prisoner object
            }
            


            loggedIn = newGuard.signIn();// set our boolean login to the output of the guard class's function signIn

            Console.WriteLine("Prisoner database: "); // Output to user to give clarity
            capeCounty.listAll(); //call the prison class's listAll function


            while (loggedIn)// run while the user is logged in 
            {
                Console.WriteLine("1. Add prisoner\t2. Assign cell\t3. Reassign Security\t 4. Prisoner Inquiry" +
                                    "\t5. Print all Prison data\n6. Early Release\t7. Check Holding\t8. Quit job\t9. Logout");
                //prompt user for input 

                while (!int.TryParse(Console.ReadLine(), out verifiedInput)) // while input cannot be parsed into an integer
                {
                    Console.WriteLine("ENTRY INVALID"); //Update user on failed input
                    Console.WriteLine("1. Add prisoner\t2. Assign cell\t3. Reassign Security\t 4. Prisoner Inquiry" +
                                        "\t5. Print all Prison data\n6. Early Release\t7. Check Holding\t8. Quit job\t9. Logout");
                    // provide the user prompt again
                }

                
                switch (verifiedInput) // switch case working on the contents of verifiedInput
                {
                    case 1: //if 1. Add prisoner
                        capeCounty.addPrisoner();// call the prison class's addPrisoner function to add prisoner
                        break;
                    case 2: //if 2. Assign cell
                        capeCounty.assignCell(); // call the prison class's assignCell function to reassign cell number
                        break;
                    case 3: //if 3. Reassign Security
                        capeCounty.changeSecurity(); // call the prison class's changeSecurity function to change a prisoners security level
                        break;
                    case 4://if 4. Prisoner Inquiry
                        capeCounty.inquiry(); // call the prison class's inquiry function to send a prisoner inquiry
                        break;
                    case 5: //if 5. Print all Prison data
                        Console.WriteLine("Prison data: ");
                        capeCounty.listAll(); // call the prison class's listALL function to list prisoner data
                        break;
                    case 6: // if 6. Early Release
                        capeCounty.deletePrisoner(); // call the prison class's delete Prisoner function to delete a prisoner
                        break;
                    case 7: //if 7. Check Holding
                        capeCounty.listHolding(); // call the prison class's listHolding function to list prisoners still in cell 0
                        break;
                    case 8:
                        Console.WriteLine("You can try..."); //funny print out to users who try to quit
                        loggedIn = !newGuard.quitJob(); // call the guards quitJob function and set loggedIn to the opposite of its output
                        break;
                    case 9://if  8. Logout 
                        Console.WriteLine("See you next shift"); //update user on their decision
                        loggedIn = false; // set loggedIn to false;
                        break;
                    default:
                        Console.WriteLine("Invalid Entry");// user did not choose a valid number
                        break;
                }


            }
           


        }


        public class Prison //create the class Prison
        {
            List<Prisoner> prison = new List<Prisoner>(); //Initialize a list of prisoners called prison

            public void simpleAddPrisoner()  // simple add Prisoner function to quickly add new prisoners
            {
                prison.Add(new Prisoner()); // add a prisoner to the list prison using the no arg prisoner constructor
            }


            public Guard hireGuard() // function used to add a guard to the prison
            {

                Console.WriteLine("We are looking to hire a new guard. What is your name? "); // prompt user for input
                String name = (Console.ReadLine()); // read input into the String name

                Random random = new Random(); //create a new Random
                int id = random.Next(2000, 5000); // creates a random ID in between the given bounds

                Console.WriteLine("You seem qualified, you're hired. Your ID number is " + id);// output guard ID to user

                Guard guard = new Guard(); // create a new guard object
                guard.name = name; //sets the guards name
                guard.guardNumber = id; // sets the guard id

                return guard; // function returns the new guard after it is created
            }

            public void addPrisoner() // prisons interactive add prisoner function
            {
                String name; // initialize name
                String security; // initialize security

                Console.WriteLine("Enter prisoner name: "); // prompt user for input
                name = Console.ReadLine(); // read user input into name
                Console.WriteLine("Enter prisoner security (min,med,max): "); // prompt user for input
                security = Console.ReadLine(); // read user input into sec
                prison.Add(new Prisoner(name, security));
            }

            public void deletePrisoner() // delete prisoner function
            {
                int verifiedID;// initialize verifiedID

                Console.WriteLine("Enter prisoner ID: ");// prompt user for input


                if (int.TryParse(Console.ReadLine(), out verifiedID)) // returns boolean value based on success of trying to parse user input into int, then stores parsed int or 0 in verified ID 
                {
                    Prisoner temp = searchByID(verifiedID);// stores the prisoner returned by the function searchByID when passed an ID into temp
                    Console.WriteLine(temp.name + " Released for.... Good Behavior?");// outputs the success
                    prison.Remove(temp);// calls the prisons remove function and passes the prisoner as an argument
                }
                else // User input could not be parsed into an integer
                {
                    Console.WriteLine("Prisoner release failed."); // update user on failure
                }

            }

            public void changeSecurity()
            {
                int verifiedID;

                Console.WriteLine("Enter prisoner ID: "); // prompt user for input
                String ID = Console.ReadLine(); // Store user input in ID
                Console.WriteLine("Enter new Security assignment: "); // prompt user for input
                String securityAssignment =Console.ReadLine(); //store user input in security assignment

                if ((int.TryParse(ID, out verifiedID)) // if the entered ID can be parsed as an Int
                    && ( securityAssignment == "max" || securityAssignment == "min"|| securityAssignment == "med")) // and the user entered a valid security assignment
                {
                    Prisoner temp = searchByID(verifiedID); //then get the prisoner associated with the id using searchByID
                    temp.secLevel = securityAssignment; // store user ented security assignment into prisoner sevLevel attribute

                }
                else //if the users entry did not meet expectations
                {
                    Console.WriteLine("Bad input. Security Reassignment failed."); // update user on failure
                }
            }

            public void printPrisonerData(Prisoner person) //function to print out a pisoners data that takes a prisoner as an argument
            {
                Console.WriteLine("Name: " + person.name + " | Sentence: " + person.sentence + " | Cell: " // printing out a prisoners attributes to the user
                                 + person.cellNum + " | Security: " + person.secLevel +" | ID: " + person.prisonID); 
            }
            public void listAll() // ListALL function lists the attributes of all prisoners
            {
                foreach (Prisoner person in prison) // for each person in the list prison
                {
                    printPrisonerData(person); // call the prison prisoner data function passing each prisoner as an argument
                }

            }

            public void listHolding() // List prisoners still in cell 0 
            {
                Console.WriteLine("Prisoners waiting in holding: "); // Print info out to user

                foreach (Prisoner person in prison) // goes through each prisoner in prison
                {
                    if(person.cellNum == 0) // if the person's cell is 0
                    {
                        Console.WriteLine("Name: " + person.name + " ID : " + person.prisonID); // write their name and ID
                    }
                }
            }
            public void assignCell() // the assign cell functions prompts the user for prisoner ID and a new cell in order to reassign a prisoners cellNum attribute
            {

                int verifiedID, verifiedCell; // initialize verifiedID and verifiedCell

                Console.WriteLine("Enter expected prisoner ID: "); // prompt user for input
                int.TryParse(Console.ReadLine(), out verifiedID); //Try to parse user input into an integer and store the integer into verified ID
                Console.WriteLine("Enter cell assignment: "); // prompt user for input
                int.TryParse(Console.ReadLine(), out verifiedCell);//Try to parse user input into an integer and store the integer into verified cell

                if (verifiedID !=0 && verifiedCell != 0) // if neither of the user inputs are 0 nor an invalid id of 0 or a non parsable value was entered 
                {
                    Prisoner temp = searchByID(verifiedID); // create a temp variable to hold the prisoner returned by search by ID when passed an ID
                    temp.cellNum = verifiedCell; // change that prisoners cell number to the user given input
                    Console.WriteLine(temp.name + " successfully reassigned."); // Update user on success
                }
                else // either a 0 or non parable int was entered
                {
                    Console.WriteLine("Bad input. Failed to assign cell."); // update user on failure to assign cell
                }

            }

            public void inquiry() // finds prisoner information when given an ID
            {
                int verifiedID; // initialize verifiedID
                Console.WriteLine("Enter prisoner ID"); // prompt user for input

                if (int.TryParse(Console.ReadLine(), out verifiedID)) // if user enters a valid int 
                {
                    
                    Prisoner temp = searchByID(verifiedID); // store prisoner in temp returned by search by ID
                    Console.WriteLine("Results: "); // Updates user on function
                    printPrisonerData(temp); // call the print prisoner data function passing the prisoner as argument
                }
                else // if the user entered something that couldnt be parsed into an integer
                {
                    Console.WriteLine("Bad input. Inquiry Failed."); // Update user on failure
                }

            }

            public Prisoner searchByID(int ID) //takes an Integer as argument to find the prisoner associated with an ID
            {
                Boolean isFound = false; // initialize isFound to false
                int verifiedID; // initialize verified ID
                while (!isFound)// while isFound is false
                {
                    foreach (Prisoner person in prison) // for each prisoner in the list prison
                    {
                        if (person.prisonID == ID) // checks prisoners id to find a match
                        {
                            isFound = true; // set is found to true
                            return person; // returns the prisoner found
                        }

                    }

                    Console.WriteLine("No prisoner matches entered ID. Retry ID entry: "); // prompt user for input
                    int.TryParse(Console.ReadLine(), out verifiedID); // try to parse user entry as integer and store in verifiedID
                    ID = verifiedID; // set ID to verifiedID

                }

                // if there are no matches update user
                throw new Exception("There is no prisoner by that ID to return"); // throw exception so that the function doesn't have an error for no return value
            }



        }


        static Random rnd = new Random();// create a new Global Random 
        public static int nextId = 3123;// initialize global variable nextID and set it to a big number
        public class Prisoner // create the Prisoner Class
        {
            //prisoner attributes

            String[] names = {"Mason","Axe", "Ted", "Peaknuckles", "Conner", "Hansen", // create array names to hold some possible prisoner names
                            "Rigby", "Constable", "Pigsly", "Brick", "Edgar", "Fromage",
                            "Hector", "Finley","John", "Hecker", "Ricky", "Charizard", "Mac",
                            "Frenchie", "Hogan", "Cena", "Craig", "Dusty", "Ben" };
            public String name; // initialize a name attribute
            public int sentence = rnd.Next(1,20); // gets a random sentence from from the bounds 1 to including 19
            public int cellNum; //  intialize a cellNum attribute
            public String secLevel; // initialize secLevel attribute
            public int prisonID; // initialize prison ID attribute

            public Prisoner() // prisoner constructor that takes no args
            {
                nextId += 1; // increment the global variable nextId
                int nIDX = rnd.Next(0, 24); // get a random index to choose a name

                this.name = names[nIDX]; // set name of prisoner being created to the random index in names array
                this.sentence = rnd.Next(1, 20); // gets a random sentence from from the bounds 1 to including 19
                this.cellNum = 0; // set cell num to 0 (the holding cell)
                this.secLevel = "min"; // set security level to min
                this.prisonID = nextId; // set prisonID to nextID

            }
            public Prisoner(String name, String sec) // prisoner constructor that takes a name and security level as argument
            {
                nextId += 1; // increment  nextId
                this.name = name; // set name of prisoner being created to name passed as argument
                this.sentence = rnd.Next(1, 20);
                this.cellNum = 0; // set cell num to 0
                this.secLevel = sec; // set prisoner being created seclevel to the sec level passed as argument
                this.prisonID = nextId; // set id to nextId
            }

        }

        public class Guard
        {
            public String name; // intialize guard attribute name
            public int guardNumber; // initalize guard attribute guard number


            public Boolean signIn() // sign in function that returns a boolean to sign a guard in
            {
                String input; // initalize input
                int userID; // intitialize userID

                Console.WriteLine("Enter id to clock in Officer " + this.name); // prompt user for input
                input = Console.ReadLine(); // store user input into String input
                int.TryParse(input, out userID);  // attempt to parse input into an int and store it into userID, on failure it stores 0 


                while (userID != this.guardNumber) // while the user input doesn't match guards id
                {
                    Console.WriteLine("WRONG ID\nEnter id to clock in Officer " + this.name); // Inform user of failure then prompt user again
                    input = Console.ReadLine(); // store user input into variable input
                    int.TryParse(input, out userID); //attempt to parse input as int and store in userID

                }

                Console.WriteLine("Login Successful"); // Update user on successful login
                return true; // return true

            }

            public Boolean quitJob() // quit job function to allow the user to try and quit their job
            {
                Boolean isQuit = false; // initialize isQuit to false
                Console.WriteLine("Are you sure you want to quit"); // prompt user for input
                String input = Console.ReadLine(); // store user input in variable input

                while(!isQuit) { // while is quit is false to keep user in a loop for awhile
                    switch (input) // switch on input contents
                    {
                        case "yes": // if user enters yes
                            Console.WriteLine("Really sure?"); // prompt user for input again
                            input = Console.ReadLine(); // get user input again
                            break;
                        case "no": // if no
                            Console.WriteLine("Glad you decided to stick with us"); // update user on their decision
                            return false; // return the function with false
                            break;
                        case "YES":// yes yes in all caps
                            Console.WriteLine("No need to shout. You were always free to leave"); // Funny output to user
                            isQuit = true; // break out of loop by setting isQuit to true
                            break;
                        default: // otherwise
                            Console.WriteLine("Thats not really an answer"); // prompt user for input that is valid
                            input = Console.ReadLine(); // read user input into variable input again
                            break;
                    }
                }


                return true;// return function with true
            }

        }

    }
}