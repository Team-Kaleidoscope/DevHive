export class AppConstants {
  public static BASE_API_URL = 'http://localhost:5000/api';
  public static API_USER_URL = AppConstants.BASE_API_URL + '/User';
  public static API_USER_LOGIN_URL = AppConstants.API_USER_URL + '/login';
  public static API_USER_REGISTER_URL = AppConstants.API_USER_URL + '/register';

  public static FETCH_TIMEOUT = 500;
}
