export class AppConstants {
  public static BASE_API_URL = 'http://localhost:5000/api';

  public static API_USER_URL = AppConstants.BASE_API_URL + '/User';
  public static API_USER_LOGIN_URL = AppConstants.API_USER_URL + '/login';
  public static API_USER_REGISTER_URL = AppConstants.API_USER_URL + '/register';

  public static API_LANGUAGE_URL = AppConstants.BASE_API_URL + '/Language';
  public static API_TECHNOLOGY_URL = AppConstants.BASE_API_URL + '/Technology';

  public static API_POST_URL = AppConstants.BASE_API_URL + '/Post';
  public static API_FEED_URL = AppConstants.BASE_API_URL + '/Feed';

  public static FALLBACK_PROFILE_ICON = 'assets/images/feed/profile-pic.png';
}
