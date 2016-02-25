using Microsoft.VisualStudio.TestTools.UnitTesting;
using CasualRacer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace CasualRacer.Model.Tests {
    [TestClass()]
    public class TrackTileExtensionsTests {
        [TestMethod()]
        public void IsGoalTile_TileIsGoal() {


            //Arrange 

            var myTile = new TrackTile();
            myTile = TrackTile.GoalLeft;

            //Act 


            bool result = myTile.IsGoalTile();
            
            //Assert 
            
            Assert.AreEqual(true, result);

            
        }
        [TestMethod()]
        public void IsGoalTile_TileIsNoGoal() {


            //Arrange 

            TrackTile myTile = TrackTile.Dirt;
            

            //Act 
            
            bool result = myTile.IsGoalTile();

            
            //Assert 

            Assert.AreEqual(false, result);


        }
    }
}