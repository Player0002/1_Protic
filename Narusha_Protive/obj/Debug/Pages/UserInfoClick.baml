   M S B A M L   `   `   `  ����J  FNarusha_Protive, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:Narusha_Protive.PagesNarusha_Protive.Pages  5      )    #Narusha_Protive.Pages.UserInfoClick   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      5local#clr-namespace:Narusha_Protive.Pages     5        N� Title$  UserInfoClick��5	      .��5
      �  J� Effect 5   
   3  -System.Windows.Media.Effects.DropShadowEffect  5         	Direction$
 -135Q�6      Opacity$	 0.3Q�60    � RowDefinitions 5   
   �� 5      $
��0.6* �6   �� 5      $��1* �6   .+�5   
   �� -   5         ��Name$ BackgroundImage��5      $D��>/Narusha_Protive;component/Resources/Login_page/Background.png��6)   $
��Fill=�6q   � 5   
   $��1��6   
������� 5      �� 5      $
��0.3* �6    �� 5      $��1* �6    �� 5      $
��0.5* �6    .+�5      �� $��2��6   $��0,1,0,0q�6"   
�������6<   � 5      $��1��6   .+�5      �� -   5         ��Name$ UserName��5      $B��<가가가가가가가가가가가가가가가가가가가가��6*   $��16(�6I   $��Center=�6W   $��Center=�6t   $��0, -40, 0,0 q�6�   �� 5      -   5        $ TeamName��5      $B��<가가가가가가가가가가가가가가가가가가가가��6*   $��14(�6I   $��Center=�6W   $��0, 20, 0, 0q�6t   
�������$��Center=�6�   5       � 5%      $��2��6    � ColumnDefinitions 5&      �� 5'      $��1* �6'   �� 5(      $��1* �6'   �� 5)      $��1* �6'   .+�5+      � -   5         �Name$ Settings��5+      	 � ForceCursor	 . 
 � Cursor$

 Hand��6<    5,      �� 5-      $
��0.5* �6(   �� 5.      $��1* �6(   �� 5/      $
��0.5* �6(   .+�51      �� $��1��6   $��Center=�6)   $��Center=�6F   $7��1/Narusha_Protive;component/Resources/settings.png��6a   52      � 53      -   5        $ 	Undefined��53      $��1��6*   	 . $

 Hand��6M    54      �� 55      $
��0.5* �6(   �� 56      $��1* �6(   �� 57      $
��0.5* �6(   .+�59      �� $��1��6   $��Center=�6)   $��Center=�6F   $;��5/Narusha_Protive;component/Resources/FileIcos/txt.png��6a   5:      � 5;      -   5        $ Logout��5;      $��2��6'   	 . $

 Hand��6J    5<      �� 5=      $
��0.5* �6(   �� 5>      $��1* �6(   �� 5?      $
��0.5* �6(   .+�5A      �� $��1��6   $��Center=�6)   $��Center=�6F   $5��//Narusha_Protive;component/Resources/logout.png��6a   5B      �� 5C      $��0,1,0,0q�6   
���   �$��3��6G   5E      5F      � 5I   
   -   5        $ UserProfile��5I      $��2��6%    5J      �� 5K      $��1* �6#   �� 5L      $
��0.6* �6#   �� 5M      $��1* �6#    5O      �� 5P      $��0.65* �6    �� 5Q      $
��0.6* �6    �� 5R      $��1.35* �6    .+�5T      A� $��1��6   $��1��6#   
�������63   A� 5U      $��1��6   $��1��6#   ��5V      �� 5W       j� Stretch$ UniformToFill=�6!    �� ImageSource 5X      �� 5Y       �� 	UriSource$7 1/Narusha_Protive;component/Resources/UserIcon.png?�6*   5[      5]      5^      5_      5`      