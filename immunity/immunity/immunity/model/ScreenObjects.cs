using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace immunity
{
    class ScreenObjects
    {
        // Gui 
        private int width;
        private int height;
        private Gui actionBar, topBar;
        private Button buttonOne, buttonTwo, buttonThree;
        private MessageHandler toast;

        // Map objects
        private Map mapObject;

        // Player objects
        private Player playerObject;

        // Unit objects
        private List<Unit> unitList;
        private List<Unit> unitsOnMap;
        private int[] units = { 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 };
        private int spawnDelay;
        private int lastUsedUnit;
        
        public ScreenObjects(int width, int height)
        {
            this.width = width;
            this.height = height;

        }

        public Player PlayerObject
        {
            get { return playerObject; }
        }

        public Gui ActionBar
        {
            get { return actionBar;}
        }

        public Gui TopBar
        {
            get { return topBar; }
        }

        public Button ButtonOne
        {
            get { return buttonOne; }
        }

        public Button ButtonTwo
        {
            get { return buttonTwo; }
        }

        public Button ButtonThree
        {
            get { return buttonThree; }
        }

        public Map MapObject
        {
            get { return mapObject; }
        }

        public MessageHandler Toast
        {
            get { return toast; }
        }

        public void Initialize()
        {
            // Gui
            toast = new MessageHandler(width, height);
            buttonOne = new Button(new Rectangle(5, height - 65, 60, 60), 10, "Basic ranged tower, low damage, single target.");
            buttonTwo = new Button(new Rectangle(70, height - 65, 60, 60), 20, "Basic splash tower, high damage, multiple targets.");
            buttonThree = new Button(new Rectangle(135, height - 65, 60, 60), 3, "Deletes a tower, 50% gold return for normal towers, 100% for walls.");
            topBar = new Gui(new Rectangle(0, 0, width, 24));
            actionBar = new Gui(new Rectangle(0, (height - 70), width, 70));

            // Map
            mapObject = new Map();

            // Player
            playerObject = new Player(5, 1000, ref mapObject);

            // Enemy
            unitsOnMap = new List<Unit>();
            unitList = new List<Unit>();
        }
    }
}
