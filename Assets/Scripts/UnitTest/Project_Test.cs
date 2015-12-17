using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class Project_Test : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
        StartConstructing_Test_Constructing();
        StartConstructing_Test_Projectlevel();
        StartConstructing_Test_ConstructionDays();
        StartConstructing_Test_Citizen();
    }

    private void StartConstructing_Test_ConstructionDays()
    {
        // Setup
        GameObject house = GameObject.Find("PlaceholderHouses");
        Houses houseScript = house.GetComponent<Houses>();

        // Test
        houseScript.StartConstructing();

        int actual = houseScript.constructionDays;

        // Assert
        const int expected = 2;

        Assert.AreEqual(actual, expected, "The constructiondays don´t match.");
    }

    private void StartConstructing_Test_Constructing()
    {
        // Setup
        GameObject house = GameObject.Find("PlaceholderHouses");
        Houses houseScript = house.GetComponent<Houses>();

        // Test
        houseScript.StartConstructing();

        bool actual = houseScript.constructing;

        // Assert
        const bool expected = true;

        Assert.AreEqual(actual, expected, "The boolean constructing didn´get set to true.");
    }

    private void StartConstructing_Test_Projectlevel()
    {
        // Setup
        GameObject house = GameObject.Find("PlaceholderHouses");
        Houses houseScript = house.GetComponent<Houses>();

        GameObject game = GameObject.Find("TinyBuilder");
        Game gameScript = game.GetComponent<Game>();

        // Test
        houseScript.StartConstructing();
        gameScript.NextDay();
        gameScript.NextDay();

        int actual = houseScript.projectLevel;

        // Assert
        const int expected = 1;

        Assert.AreEqual(actual, expected, "The Project didn´t get upgraded.");
    }

    private void StartConstructing_Test_Citizen()
    {
        // Setup
        GameObject house = GameObject.Find("PlaceholderHouses");
        Houses houseScript = house.GetComponent<Houses>();

        GameObject game = GameObject.Find("TinyBuilder");
        Game gameScript = game.GetComponent<Game>();
        gameScript.GameReset();

        // Test
        houseScript.StartConstructing();
        gameScript.NextDay();
        gameScript.NextDay();

        int actual = Game.overseer.citizen;

        // Assert
        const int expected = 10;

        Assert.AreEqual(actual, expected, "The Project didn´t increased the value of the citizen.");
    }
}