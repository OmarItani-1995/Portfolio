# Portfolio
A small portfolio of what i've learned over the years. It's not fully documented yet, but will be done soon. 
Will add small Systems and Mechanics over time so stay tuned!

Content: 

1. Dependency Injection Demo
   Uses generics to register singletons and a group of classes, it has a small Test Scene that Debugs the results of the calls when you run the Scene.

   You can add an object as a singleton:
   
       Di.AddSingleton<Singleton_Test>();

   You get back the singleton using this call:

       Di.Get<Singleton_Test>();

   You can also add a singleton using an interface:

       Di.AddSingleton<IInterfaceTest, Interface_Test>();

   Or add a base class and get all its children as a group (Uses Reflection):

       Di.AddGroup<Test_Group>();

3. Event System Demo
   Registers all event listeners on the start of the game using reflection, and then creates and calls them when an event of a specific type occurs.

   If we have a test event:
   
       public class TestEvent
       {
          public string Name { get; set; }
       }
   
   You only need to implement Event_Listener with TestEvent as its argument and the rest is automated.

        public class TestEventListener_First : Event_Listener<TestEvent>
        {
            public override void OnEvent(TestEvent ev)
            {
                Debug.Log($"First Event Listener: {ev.Name}");
            }
        }

  And then whenever you call fire event with TestEvent as an argument all event listener's OnEvent function will be called. 
  
       EventSystem.FireEvent(new TestEvent { Name = "Test Event" });

4. Lerper
   A fast runtime lerping API, It can be extended to lerp any type: Color, Vector3, floats
   You can use it for a quick time specific lerping of a component, it also has a callback for one its done so you can chain lerpers, or continue with the sequence.

5. Manipulator
   A component Manipulator, it can be used specially for prototypes to quickly iterate over animations and effects, similar to DoTween.
   It can be initialized in the Editor and Runtime, and it can be saved as template for later use.
   It can also be chained with more manipulators.
   Currently there is only MovePosition and Scale for demo purposes.
   I'm using builder pattern for manipulators initialization.
   Example:
   
        TestingTransform
            .Manipulate(duration: 2, manipulator =>
            {
                manipulator
                    .Add<MWorker_MoveToPosition>(worker =>
                    {
                        worker.SetDestination(TestingTransform.localPosition + Vector3.forward * 10).UseSpace(Space.Self);
                    })
                    .Add<MWorker_ScaleTo>(worker =>
                    {
                        worker.SetEndScale(Vector3.one * 2);
                    });
            })

   In this example we have:
   1. You call the Manipulate function extention on any transform that you want to manipulate
   2. You specify the duration of the manipulator
   3. The second argument is an action of the created manipulator, you can use it to initialize the manipulator with workers
   4. You call Add function on the manipulator to add any type of workers, you can create your own worker by implementing the class MWorker
   5. For each worker that you add, you get an intialization action to build the worker.

  It works well for quick iterations on the effect, it can be integrated with other Modules, like Particle Systems and Cameras to apply effects and shakes. 
  Future Updates: Using Code Generation to bake the manipulator into a one optimized class instead of nesting classes. 
     

6. Msg System
   A messaging system using Generics for easy message Listen/Queue.
   You subscribe to msgs by calling Msg.Listen with the type of message as a generic argument, and the Action of that msg: 

         Msg.Listen<Msg_TestMessage>((msg) => Debug.Log(msg.GetType().Name)); // will debug Msg_TestMessage

   You also Queue msgs by calling Msg.Queue with a message instance as an argument:

         Msg.Queue(new Msg_TestMessage()
         {
           Data = "Test Data"
         }); // Should Print Test Data

   A message can have more than One subscriber, it only needs to be a child of Message class.
   Few More Updates coming soon: Initialization callback for singletons, debugging message containers, permenant listeners queue.
   
