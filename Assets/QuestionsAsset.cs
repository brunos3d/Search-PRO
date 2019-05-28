using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Answer {

	public string text;
	public bool correct;
	public int questionTime;

	public Answer(string text, bool correct, int questionTime) {
		this.text = text;
		this.correct = correct;
		this.questionTime = questionTime;
	}
}

[CreateAssetMenu(fileName = "new Question", menuName = "Question", order = 81)]
public class QuestionsAsset : ScriptableObject {

	public string question;

	public List<Answer> answers = new List<Answer>();
}
