import { LoggedInUser } from "../interfaces/user";

export class Mappers {

    /**
     * Converts a JWT token into a LoggedInUser object.
     * Extracts specific claims based on their URIs used as keys in the token's payload.
     * @param decodedToken - The object representing the decoded JWT token.
     * @returns {LoggedInUser} - A new instance of LoggedInUser populated with values from the token.
     */
    static mapDecodedTokenToLoggedInUser(decodedToken: any): LoggedInUser {
        return {
            id: decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"],
            email: decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"],
            role: decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"],
            firstname: decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"],
            lastname: decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname"]
        };
    }
}
