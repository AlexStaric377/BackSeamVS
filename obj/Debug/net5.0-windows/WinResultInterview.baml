   M S B A M L   `   `   `  ����D  @BackSeam, Version=1.0.1.62, Culture=neutral, PublicKeyToken=null#clr-namespace:BackSeamBackSeam  5	   	   !    BackSeam.WinResultInterview   5      W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5   	   "localclr-namespace:BackSeam  5	   	     � Title$  WinResultInterview��5   	   $	��480��6$   $	��690��61    � WindowState$ Normal=�6=    � AllowsTransparency$
 True��6R    � WindowStyle$
 None=�6l    � WindowStartupLocation$ CenterScreen=�6    � ShowInTaskbar$
 True��5   
   $��0q�6"   $��Stretch=�6-   $��Stretch=�6K   $��Microsoft Sans Serif)�6g   ��6�   Z�  � DataContext 5      '   !BackSeam.ViewModelResultInterview  5   
   .��5      � $	��450��6   $	��650��6   $��Center=�6$   .+�5      �� -   5         ��Name$ 	ButtonWin��5      $��Center=�6-   $	��401��6H   $	��630��6U    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5      	   Color$	 	#FF1D1504��6#   
   
BlurRadius$
 45Q�65      ShadowDepth$ 20Q�6E      Opacity$	 0.5Q�6V   6g   .+�5      �� -   5         ��Name$ 	BorderWin��5      ���	#FFCED7F7���	#FFCED7F7$	��400��6W   $	��630��6d   $��0,0,0,0q�6q    �� CornerRadius$ 15,15,15,15��6�   ._�5      �� $��Center=�6   $	��370��68   $	��630��6E   ���	#FFCED7F7 6h   �� *SelectedResultInterview+.+�5      � -   5        $��GridResultInterview��5      ���	#FFCED7F7$��Center=�6N   $	��Top=�6k   $	��370��6�   $	��630��6�    � RowDefinitions 5      �� 5      $	��50* �6,   �� 5      $
��350* �6,   �� 5      $	��70* �6,   .+�5      � $��0��6   .+�5      �� -   5        $��DateInterview��5   $   $��30��6�   $	��120��6�   $��14(�6�   $��Microsoft Sans Serif)�6�   $��10,0,0,0q�6�   $	��Top=�6�   $��Center=�6�   $
��Left=�6  ��6:   ��  �� Path$ dateInterview� �� UpdateSourceTrigger$ PropertyChanged=�6  �� 5!      -   5        $��	Zagolovok��5!   $   $^��XВисновки за резльтатами проведеного опитування��66   $��0��6o   $��30��6|   $��16(�6�   $��Microsoft Sans Serif)�6�   $	��Top=�6�   $��Center=�6�   $��Center=�6�   $��20,0,0,0q�6  �� 5"      ���	#FF979595$��1,1,1,1q�6=   $��30��6W   $��30��6c   $��0,0,5,0q�6n   $��Right=�6   $��Center=�6�   ._�5#   "   �� ���	#FFCED7F7$��30��6a   $��30��6m   $��Center=�6y   ���	#FFCED7F7��6)   �� *CloseResult+.��5$   &   �� $B��<pack://application:,,,/BackSeam;component/Images/delete2.png��6-   $��24��6t   $��24��6�   6�   5%   #   5&      �� 5'      ���	#FF979595$��1,1,1,1q�6=   $��30��6W   $��30��6c   $��0,0,40,0q�6n   $��Right=�6�   $��Center=�6�   ._�5(   "   �� ���	#FFCED7F7$��30��6b   $��30��6n   $��Center=�6z   ���	#FFCED7F7��6)   �� *PrintDiagnoz+.��5)   &   �� $B��<pack://application:,,,/BackSeam;component/Images/printer.png��6-   $��24��6t   $��24��6�   6�   5*   #   5+      5,      � 5.      $��1��6    � ColumnDefinitions 5/      �� 50      $	��86* �6/   �� 51      $
��239* �6/    53      �� 54      $	��10* �6,   �� 55      $	��60* �6,   �� 56      $	��80* �6,   �� 57      $	��80* �6,   �� 58      $	��70* �6,   �� 59      $	��70* �6,   .+�5>      �� -   5         ��Name$ NameInterview��5>   (   $��1��6?   $
��Wrap=�6L   $��14(�6`   $'��!  Назва опитування��6n   $��Microsoft Sans Serif)�6�   $	��152��6�   $
��Left=�6�    �� TextAlignment$
 Left=�6�   $��40��6�   �� 5?      -   5        $��BoxNameInterview��5?   '    �� 
IsReadOnly$
 True��6�   $��1��6�   $��1��6�   $��Microsoft Sans Serif)�6�   ���	#FFCED7F7$��Center=�5@   R   $��40��6m    �� TextWrapping$
 Wrap=�6y   $	��310��6�   $��14(�6�   $
��Left=�6�   $��0,0,0,0q�6�   ��5?   ?   �� $ nametInterview�$ PropertyChanged=�5@   �   �� 5A      -   5        $ ListInterview��5A   (   $��1��6?   $��1��6L   $+��%     Зміст
 опитування��6\   $��Microsoft Sans Serif)�6~   $��14(�6�   $��40��6�   $��90��6�   $��Right=�6�   $	��Top=�6�   $��0,0,50,0q�6�   $y�Visible=�6  �� 5B      -	   5         ��Name$ FolderListInterview��5B       $B��<pack://application:,,,/BackSeam;component/Images/folder_.png��6=   $��1��6�   $��1��6�   $��25��6�   $	��Top=�6�   $��Right=�6�   $��0,0,20,0q�6�   $y�Visible=�6�   *  $Microsoft.Xaml.Behaviors.Interaction   Triggers 5C      +  %Microsoft.Xaml.Behaviors.EventTrigger  5D   "      	EventName$ MouseLeftButtonDown��61   *  $Microsoft.Xaml.Behaviors.TriggerBase   Actions. 5E   *   2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction     CommandParameter$  ��6�      Command 6@   ��  �� ElementName$ GridResultInterview��$' !DataContext.ReadIntreviewProtokol�6�   5F   #   5H      �� 5I      -
   5        $ NameDiagnoz��5I   (   $��2��6=   $
��Wrap=�6J   $��0��6_   $��60��6o   $>��8  Імовірний попередній діагноз��6{   $��Microsoft Sans Serif)�6�   $	��173��6�   $
��Left=�6�   $��Bottom=�6�   $��14(�6  �� 5J      -   5        $��BoxNameDiagnoz��5J   m   $��2��6�   $��1��6�   $��65��6�   $��14(�6�   $��Microsoft Sans Serif)�6�   $
 Wrap=�5K   :   ���	#FFCED7F7$	��Top=�6e   $	��400��6�   $
��Left=�6�   $��0,0,0,0q�6�   $
 True��6�   ��5J   &   �� $ nameDiagnoza�$ PropertyChanged=�5K   �   �� 5L      -   5        $ 	ShortOPis��5L   (   $��3��6;   $N��H  Короткий опис
  попереднього діагнозу ��6H   $��Microsoft Sans Serif)�6|   $��14(�6�   $��32��6�   $	��165��6�   $
��Left=�6�   $��Center=�6�   �� 5M      -   5        $��BoxInterviewOpis��5M   &   $
 Wrap=�6�   $��1��6�   $��3��6�   $
 True��6�   ���	#FFCED7F7$	��450��5N   =   $��65��6I   $��14(�6U   $
��Left=�6c   $��0,0,0,0q�6~   $��Microsoft Sans Serif)�6�   ��5M   >   �� $ opistInterview�$ PropertyChanged=�5N   �   �� 5O      -   5        $ NameRecomen��5O   (   $��0��6=   $��4��6M   $H��B  Рекомендації щодо
     подальших дій��6Z   $��Microsoft Sans Serif)�6�   $
��Wrap=�6�   $��14(�6�   $��50��6�   $	��165��6�   $
��Left=�6�   $��Bottom=�6  �� 5P      -   5        $��BoxNameRecomen��5P   '   $��1��6�   $��4��6�   ���	#FFCED7F7$��Microsoft Sans Serif)�6�   $
 Wrap=�5Q   :   $	��400��6N   $��50��6Z   $��14(�6f   $
��Left=�6t   $��0,0,0,0q�6�   $
 True��6�   ��5P   =   �� $ nameRecommendation�$ PropertyChanged=�5Q   �   �� 5S      -   5        $ UriInterview��5S   (   $��5��6>   $'��!  Опис в інтернеті ��6K   $��Microsoft Sans Serif)�6f   $��14(�6�   $��24��6�   $	��165��6�   $
��Left=�6�   $��Center=�6�   �� 5T      -   5        $��TextUriInterview��5T   '   $
 True��6�   $��5��6�   $��1��6�   $��Microsoft Sans Serif)�6�   ���	#FFCED7F7$��Center=�5U   >   $��32��6Y   $
 Wrap=�6e   $	��400��6y   $��14(�6�   $
��Left=�6�   $��0,0,0,0q�6�   ��5T   ?   �� $ uriInterview�$ PropertyChanged=�5U   �   �� 5V      -   5        $ RunUriInterview��5V       $B��<pack://application:,,,/BackSeam;component/Images/folder_.png��69   $��1��6   $��5��6�   $��25��6�   $��30��6�   $��Right=�6�   $��0,0,20,0q�6�   $y�Visible=�6�    5W        5X   "   $ MouseLeftButtonDown��61   . 5Y   &     $  ��6�    6<   �� $ GridResultInterview��$ DataContext.RunGoogleUri�6�   5Z   #   5\      5]      � 5^      $��2��6    5_      �� 5`   "   $
��180* �63   �� 5a   "   $
��120* �63   �� 5b   "   $
��120* �63   �� 5c   "   $
��120* �63   .+�5f      �� $��35��6&   $	��180��62   $��0��6>   
���   �
���   �$��Center=�6u   $��Center=�6�   $��1,1,1,1q�5g      $ 3,3,3,3��69    � Cursor$
 Hand��6P   ._�5h   "   �� $
��Auto��6-   
�������$
��Auto��6M    5i   &     5j   *   $ MouseLeftButtonDown��69   . 5k   .     $  ��6g    6D   �� *ReseptionLikar+6g   5l   +   .+�5n   &   �� $��2��6,   $��35��6<   $��13(�6H   ���	#FFCED7F7$	��180��6n   $4��.Запис на прийом до лікаря��6z   $��Center=�6�   $��Center=�6�   $��Center=�6�   $��Center=�6�   $��Microsoft Sans Serif)�6  5o   '   5p   #   5q      �� 5r      $��35��6&   $	��130��62   $��1��6>   
���   �
���   �$��Center=�6v   $��Center=�6�   $��1,1,1,1q�5s   #   $ 3,3,3,3��6>   $
 Hand��6U   ._�5t   "   �� $
��Auto��6-   
�������$
��Auto��6M    5u   &     5v   *   $ MouseLeftButtonDown��69   . 5w   .     $  ��6f    6D   �� *ProfilMedical+6f   5x   +   .+�5z   &   �� $��2��6,   $��35��6<   $��13(�6H   $	��130��6W   $(��"Профільні лікарні ��6c   ���	#FFCED7F7$��Center=�6�   $��Center=�6�   $��Center=�6�   $��Center=�6�   $��Microsoft Sans Serif)�6  5{   '   5|   #   5}      �� 5~      $��35��6&   $	��110��62   $��2��6>   
���   �
���   �$��Center=�6v   $��Center=�6�   $��1,1,1,1q�5   #   $ 3,3,3,3��6>   $
 Hand��6U   ._�5�   "   �� $
��Auto��6-   
�������$
��Auto��6M    5�   &     5�   *   $ MouseLeftButtonDown��69   . 5�   .     $  ��6j    6D   �� *SaveIntervDiagnoz+6j   5�   +   .+�5�   &   �� $��2��6,   $��35��6<   $��13(�6H   $	��110��6W   $��Зберегти��6c   ���	#FFCED7F7$��Center=�6�   $��Center=�6�   $��Center=�6�   $��Center=�6�   $��Microsoft Sans Serif)�6  5�   '   5�   #   5�      �� 5�      $��35��6&   $	��130��62   $��3��6>   
���   �
���   �$��Center=�6v   $��Center=�6�   $��1,1,1,1q�5�   #   $ 3,3,3,3��6>   $
 Hand��6U   ._�5�   "   �� $
��Auto��6-   
�������$
��Auto��6M    5�   &     5�   *   $ MouseLeftButtonDown��69   . 5�   .     $  ��6j    6D   �� *ContinueInterview+6j   5�   +   .+�5�   &   �� $��2��6,   $��35��6<   $��13(�6H   $	��130��6W   $%��Продовжити пошук��6c   ���	#FFCED7F7$��Center=�6�   $��Center=�6�   $��Center=�6�   $��Center=�6�   $��Microsoft Sans Serif)�6  5�   '   5�   #   5�      5�      5�      5�      5�      5�      5�      5�      