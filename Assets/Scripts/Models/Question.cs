using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestion", menuName = "Quiz/Question")]
public class Question : ScriptableObject
{
    [TextArea] public string questionText;      // Question Detail
    public string[] answers;                    // Answers
    public int indexCorrectAnswer;              // Correct Answer
    public QuestionType questionType;
}

public enum QuestionType
{
    Intelligent,
    Personal,
    ThisThat,
    Fun,
    Math,
    Foreign
}


/// <summary>
/// ScriptableObjects : Can created in assets. To create more questions for players answer
/// This Question for npc when player interact to get quest and 
/// receive reward from npc is gold tobuy something
/// </summary>