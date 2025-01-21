import { NewEvaluation } from '../interfaces/new-evaluation';
import { LoggedInUser } from '../interfaces/user';

export class Mappers {
  /**
   * Converts a JWT token into a LoggedInUser object.
   * Extracts specific claims based on their URIs used as keys in the token's payload.
   * @param decodedToken - The object representing the decoded JWT token.
   * @returns {LoggedInUser} - A new instance of LoggedInUser populated with values from the token.
   */
  static mapDecodedTokenToLoggedInUser(decodedToken: any): LoggedInUser {
    return {
      id: decodedToken[
        'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'
      ],
      email:
        decodedToken[
          'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'
        ],
      role: decodedToken[
        'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
      ],
      firstname:
        decodedToken[
          'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'
        ],
      lastname:
        decodedToken[
          'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname'
        ],
    };
  }

  /**
   * Maps evaluation answers to an array of numbered JSON objects where each object holds
   * a single numbered key and value.
   * @param evaluationAnswers - The object containing evaluation fields and their values.
   * @returns {Array<Object>} - An array where each element is an object with a single key-value pair, key being the index.
   */
  static mapToJsonSubmitEvaluation(evaluationAnswers: any): any {
    const keyOrder = [
      'selfConfidence',
      'dedication',
      'jobKnowledge',
      'qaOfWork',
      'abilityDeadlines',
      'independence',
      'commitment',
      'detailAttention',
      'workWithOthers',
      'communicationSkills',
      'underPressure',
    ];

    const resultArray = keyOrder.map((key, index) => {
      return { "QuestionId": index + 1, "AnswerValue": evaluationAnswers[key]};
    });

    return resultArray;
  }

  /**
   * Constructs a new evaluation object with given answers and participant details.
   * This function captures the current moment for timestamping the evaluation and
   * includes all necessary identifiers for the evaluation context.
   *
   * @param {any} answers - The answers for the evaluation.
   * @param {number} evaluator - The user ID of the evaluator conducting the evaluation.
   * @param {number} evaluatee - The user ID of the user being evaluated.
   * @param {number} cycle - The identifier for the evaluation cycle.
   * @returns {NewEvaluation} - An object representing a newly created evaluation, including timestamps and participant IDs.
   */
  static mapToNewEvaluation(
    answers: any,
    evaluator: number,
    evaluatee: number,
    cycle: number
  ): NewEvaluation {
    return {
      evaluateeUserId: evaluatee,
      evaluatorUserId: evaluator,
      evaluationCycleId: cycle,
      timeStamp: new Date().toISOString(),
      answers: answers,
    };
  }
}
