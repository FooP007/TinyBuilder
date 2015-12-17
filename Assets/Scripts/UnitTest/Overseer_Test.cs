using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;


public class Overseer_Test : MonoBehaviour
{
    void Start()
    {
        Solvent_Test_Broke();
        Solvent_Test_Even();
        Solvent_Test_Rich();
        Solvent_Test_BiggerDiscount();

        ActualCost_Test_SmallDiscount();
        ActualCost_Test_NormalDiscount();
        ActualCost_Test_HighDiscount();
        ActualCost_Test_TooHighDiscount();
        ActualCost_Test_TooSmallDiscount();
    }

    public void ActualCost_Test_SmallDiscount()
    {
        //Setup
        int price = 15; ;
        int discount = 5;

        //Test
        int actual = Overseer.ActualCost(price, discount);

        //Assert
        const int expected = 10;

        Assert.AreEqual(actual, expected, "Something went wrong");
    }

    public void ActualCost_Test_NormalDiscount()
    {
        //Setup
        int price = 15; ;
        int discount = 10;

        //Test
        int actual = Overseer.ActualCost(price, discount);

        //Assert
        const int expected = 5;

        Assert.AreEqual(actual, expected, "Something went wrong");
    }

    public void ActualCost_Test_HighDiscount()
    {
        //Setup
        int price = 15; ;
        int discount = 15;

        //Test
        int actual = Overseer.ActualCost(price, discount);

        //Assert
        const int expected = 0;

        Assert.AreEqual(actual, expected, "Something went wrong");
    }

    public void ActualCost_Test_TooHighDiscount()
    {
        //Setup
        int price = 15; ;
        int discount = 20;

        //Test
        int actual = Overseer.ActualCost(price, discount);

        //Assert
        const int expected = 0;

        Assert.AreEqual(actual, expected, "Something went wrong");
    }

    public void ActualCost_Test_TooSmallDiscount()
    {
        //Setup
        int price = 15; ;
        int discount = -5;

        //Test
        int actual = Overseer.ActualCost(price, discount);

        //Assert
        const int expected = 15;

        Assert.AreEqual(actual, expected, "Something went wrong");
    }

    public void Solvent_Test_Broke()
    {
        // Setup
        GameObject Whitehouse = GameObject.Find("PlaceholderWhitehouse");
        Whitehouse wScript = Whitehouse.GetComponent<Whitehouse>();
        int coins = 5;
        int discount = 0;

        // Test
        bool actual = Overseer.Solvent(wScript, coins, discount);

        // Assert
        const bool expected = false;

        Assert.AreEqual(actual, expected, "Player doesn´t have enough coins!");
    }

    public void Solvent_Test_Even()
    {
        // Setup
        GameObject Whitehouse = GameObject.Find("PlaceholderWhitehouse");
        Whitehouse wScript = Whitehouse.GetComponent<Whitehouse>();
        int coins = 10;
        int discount = 0;

        // Test
        bool actual = Overseer.Solvent(wScript, coins, discount);

        // Assert
        const bool expected = true;

        Assert.AreEqual(actual, expected, "Player has the same amount of coins that the project costs!");
    }

    public void Solvent_Test_Rich()
    {
        // Setup
        GameObject Whitehouse = GameObject.Find("PlaceholderWhitehouse");
        Whitehouse wScript = Whitehouse.GetComponent<Whitehouse>();
        int coins = 1005;
        int discount = 0;

        // Test
        bool actual = Overseer.Solvent(wScript, coins, discount);

        // Assert
        const bool expected = true;

        Assert.AreEqual(actual, expected, "Player has more then enough coins!");
    }

    public void Solvent_Test_BiggerDiscount()
    {
        // Setup
        GameObject Whitehouse = GameObject.Find("PlaceholderWhitehouse");
        Whitehouse wScript = Whitehouse.GetComponent<Whitehouse>();
        int coins       = 10;
        int discount    = 15;

        // Test
        bool actual = Overseer.Solvent(wScript, coins, discount);

        // Assert
        const bool expected = true;

        Assert.AreEqual(actual, expected, "Player has greater discount then the cost!");
    }
}