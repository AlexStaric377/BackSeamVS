   M S B A M L   `   `   `  ����D  @BackSeam, Version=1.0.1.62, Culture=neutral, PublicKeyToken=null#clr-namespace:BackSeamBackSeam  5	   	       BackSeam.WinNsiPacient   5      W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5   	   "localclr-namespace:BackSeam  5	   	     � Title$  WinNsiPacient��5   	    � WindowState$ Normal=�6    � AllowsTransparency$
 True��64    � WindowStyle$
 None=�6N    � WindowStartupLocation$ CenterScreen=�6a    � ShowInTaskbar$
 True��5   
   $	��420��6!   $	��830��6.   $��0q�6:   $��Stretch=�6E   $��Stretch=�6c   ��5   	   Z�  � DataContext 5      "   BackSeam.ViewModelNsiPacient  5   
   .��5      �� -   5         ��Name$ 	ButtonWin��5      $��Center=�6%   $	��360��6@   $	��800��6M    J� Effect 5   
   3  -System.Windows.Media.Effects.DropShadowEffect  5      	   Color$	 	#FF1D1504��6   
   
BlurRadius$
 35Q�61      ShadowDepth$ 20Q�6A      Opacity$	 0.5Q�6R   6c   .+�5   
   �� -   5         ��Name$ 	BorderWin��5      ���	#FFE2E2DD���	#FFE2E2DD$��0,0,0,0q�6T    �� CornerRadius$ 15,15,15,15��6n   $	��360��6�   $	��780��6�   ._�5      � -   5        $��WinGrid��5   e   $��Center=�6   $��Center=�60   $	��350��6K   $
��Auto��6X   ���	#FFE2E2DD � RowDefinitions 5      �� 5      $	��30* �6$   �� 5      $	��24* �6$   �� 5      $
��270* �6$   �� 5      $	��30* �6$   .+�5      �� -   5        $��	Zagolovok��5      $*��$Довідник пацієнтів ��6*   $��0��6H   $��12(�6U   $��Black"�6c   $��Center=�6v   � 5       $��1��6    � ColumnDefinitions 5!      �� 5"      $
��110* �6+   �� 5#      $
��120* �6+   �� 5$      $
��120* �6+   �� 5%      $
��120* �6+   �� 5&      $	��60* �6+   .+�5(      ��  �� Orientation$ 
Horizontal=�6!   $��1��6:   $��3��6H   .+�5)      �� -   5        $��NamePac��5)   -   $��0��6   $��50��6<   $��    Ім'я ��6H   $��12(�6\   $��Light"�6j   $
��Left=�6}   �� 5*      -   5        $��TextBoxNamePac��5*   .   $��0��6   $	��110��6�   $��Right=�6�   $��0,0,0,0q�6�   $��Center=�6�   ��6D   ��  �� Path$ NamePacient� �� UpdateSourceTrigger$ PropertyChanged=�6�   �� 5+      -   5        $��
SurNamePac��5+   ,   $��1��6   $��60��6>   $��Прізвище ��6J   $��12(�6^   $��Light"�6l   $
��Left=�6   $��Center=�6�   �� 5,      -   5        $��TextBoxSurNamePac��5,   .   $��1��6   $	��110��6�   $��Right=�6�   $��0,0,0,0q�6�   $��Center=�6�   ��6G   �� $ SurNamePacient�$ PropertyChanged=�6�   �� 5-      -	   5        $��TelPac��5-   ,   $��2��6   $��60��6:   $��Телефон ��6F   $��12(�6Y   $��Light"�6g   $
��Left=�6z   �� 5.      -
   5        $��TextBoxTelPac��5.   .   $��2��6   $	��110��6�   $��Right=�6�   $��0,0,0,0q�6�   $��Center=�6�   ��6C   �� $ TelefonPacient�$ PropertyChanged=�6�   5/      �� 51      -   5        $��
ProfAgePac��51   ,   $��3��6   $	��160��6>   $8��2Спеціальність                 Вік ��6K   $��12(�6y   $��Light"�6�   �� 52      $��4��6   $��22��6.   $��50��6:   ���	#FF3E3E3B$��Center=�6]   ���	#FFE2E2DD$��1,1,1,1q�53   5   $ 10,10,10,10��6O    � Cursor$
 Hand��6j   *  $Microsoft.Xaml.Behaviors.Interaction   Triggers 54      +  %Microsoft.Xaml.Behaviors.EventTrigger  55         	EventName$ MouseLeftButtonDown��6-   *  $Microsoft.Xaml.Behaviors.TriggerBase   Actions. 56   "   2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction     CommandParameter$  ��6Z      Command 68   �� *SearchPacient+6Z   57      ._�59      �� $��2��6    $��24��60   $��10(�6<   $��70��6K   $��
Пошук��6V   $��Center=�6f   $��Center=�6�   $��Center=�6�   $��Center=�6�   $��Arial)�6�   5:      5;      5<      � 5=      $��2��6   $	��760��6$   .+�5>      �� -   5        $��TablPacients��5>      $	��Top=�6W   $	��260��5?   ?   ���	#FFE2E2DD$	��680��5@      $��0,0,0,0q�6!   $��Hidden=�6;   
������ $
��Auto=�5A   /   ��5>   3   �� *NsiPacients+ �� SelectedItem 5?      �� *SelectedPacient+ 5B        5C      $ MouseUp��6-   . 5D   "     $  ��6�    68   ��  �� ElementName$ TablPacients��$$ DataContext.SelectTablPacients�6�   5E      ��5G      �� 5H      .R�5I      �� $ 
StackPanel��6)   $ 
Horizontal=�6>   $��5q�6W   $��30��6b   .+�5J   "   �� $��0,0,0,5q�6G   $��Center=�6X   $	��180��6t   $��20��6�   $��16(�6�   $��Black"�6�   $
��Left=�6�   ��6,   �� $
 name�6�   �� 5K   "   $��0,0,0,5q�6J   $��Center=�6[   $	��160��6w   $��20��6�   $��16(�6�   $��Black"�6�   $
��Left=�6�   ��6,   �� $ surname�6�   �� 5L   "   $��0,0,0,5q�6F   $��Center=�6W   $	��140��6s   $��20��6   $��16(�6�   $��Black"�6�   $
��Left=�6�   ��6,   �� $	 tel�6�   �� 5M   "   $��0,0,0,5q�6M   $��Center=�6^   $	��115��6z   $��20��6�   $��16(�6�   $��Black"�6�   $
��Left=�6�   ��6,   �� $ 
profession�6�   �� 5N   "   $��0,0,0,5q�6F   $��Center=�6W   $	��130��6s   $��20��6   $��16(�6�   $��Black"�6�   $
��Left=�6�   ��6,   �� $	 age�6�   5O      5P      5R      5S      �� 5U      -   5        $��
Close_Grid��5U   Y   $��20��6   $��20��6$   $��0��6/   $��Right=�6<   $��Bottom=�6l   $B��<pack://application:,,,/BackSeam;component/Images/xrestik.png��5V      $
 Hand��5W       5X        5Y      $ MouseLeftButtonDown��6)   . 5Z        $  ��6U    64   �� *ClosePacient+6U   5[      5]      � 5^      $��3��6    5_      �� 5`      $
��205* �6+   �� 5a      $
��205* �6+   �� 5b      $
��205* �6+   .+�5d      �� $��2��6   $��0��6*   $��25��6:   $��70��6F   ���	#FF3E3E3B$��Bottom=�6i   ���	#FFE2E2DD$��1,1,1,1q�5e   5   $ 15,15,15,15��6O   $
 Hand��6j    5f        5g      $ MouseLeftButtonDown��6-   . 5h   "     $  ��6Z    68   �� *SelectPacient+6Z   5i      ._�5k      �� $��2��6    $��25��60   $��10(�6<   $��70��6K   $��Вибрати��6V   $��Center=�6h   $��Center=�6�   $��Center=�6�   $��Center=�6�   $��Arial)�6�   5l      5m      �� 5n      $��2��6   $��2��6*   $��25��6:   $��80��6F   ���	#FF3E3E3B$��Bottom=�6i   ���	#FFE2E2DD$��1,1,1,1q�5o   5   $ 15,15,15,15��6O   $
 Hand��6j    5p        5q      $ MouseLeftButtonDown��6-   . 5r   "     $  ��6Y    68   �� *ClosePacient+6Y   5s      ._�5u      �� $��2��6    $��25��60   $��10(�6<   $��Normal$�6K   $��Courier)�6^   $��65��6t   $��Завершити��6   $��Center=�6�   $��Center=�6�   $��Center=�6�   5v      5w      �� 5x      -   5        $ ResultInsert��5x   ;   $��2��6   $��1��6*   $��30��6T   $��40��6`   
���   ����	#FFE2E2DD$��Center=�6�   $��Center=�6�   $��0,0,0,0q�5y      $ 3,3,3,3��69   $
 Hand��6P   ._�5{      �� -   5        $��	Insertstr��5{   p   $��30��6    $��35��6,   $��Center=�67   $��Center=�6R   $?��9pack://application:,,,/BackSeam;component/Images/add2.png��5|   *   $
 Hand��6o   6�    5}        5~      $ MouseLeftButtonDown��6-   . 5   "     $  ��6`    68   �� *PacientProfilInsert+6`   5�      5�      5�      5�      5�      5�      5�      