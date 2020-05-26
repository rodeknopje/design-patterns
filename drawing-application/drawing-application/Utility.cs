using System;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using drawing_application.CustomShapes;
using drawing_application.Strategies;

namespace drawing_application
{
    public class Utility
    {
        // the singleton of this class.
        private static Utility instance;
        // all types that derive from custom shape.
        private readonly Type[] styles;

        private Utility()
        {
            instance = this;
            // get all types that derive from custom shape.
            //styles = Assembly.GetAssembly(typeof(CustomShape)).GetTypes().Where(T => T.IsSubclassOf(typeof(CustomShape))).ToArray();
            styles = Assembly.GetAssembly(typeof(IStrategyShape)).GetTypes().Where(T => typeof(IStrategyShape).IsAssignableFrom(T) && T !=typeof(IStrategyShape)).ToArray();

        }

        public static Utility GetInstance()
        {
            return instance ?? new Utility();
        }

        public CustomShape CreateShape(int index)
        {
            // create a new shape based on the selected shape.
            return new CustomShape((IStrategyShape)Activator.CreateInstance(styles[index]));
        }

        public CustomShape CreateShape(string name)
        {
            // create a new shape based on their class name.
            //return (CustomShape)Activator.CreateInstance(styles[GetStyleIndexByClassName(name)]);
            return new CustomShape ((IStrategyShape) Activator.CreateInstance(styles[GetStyleIndexByClassName(name)]));
        }

        private int GetStyleIndexByClassName(string name)
        {
            // loop through all the different shape types.
            for (var i = 0; i < styles.Length; i++)
            {
                // if the class name matches the given name.
                if (styles[i].Name == name)
                {
                    // return i as the index.
                    return i;
                }
            }
            return 0;
        }

        public Type[] GetShapeTypes()
        {
            return styles;
        }
    }
}
