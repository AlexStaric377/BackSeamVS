   M S B A M L   `   `   `  ����D  @BackSeam, Version=1.0.1.62, Culture=neutral, PublicKeyToken=null#clr-namespace:BackSeamBackSeam  5	   	   %    BackSeam.WinColectionStanHealth   5      W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5   	   "localclr-namespace:BackSeam  5	   	     � Title$  WinColectionStanHealth��5   	   $	��570��6(   $
��1080��65    � WindowState$ Normal=�6B    � AllowsTransparency$
 True��6W    � WindowStyle$
 None=�6q    � WindowStartupLocation$ CenterScreen=�6�    � ShowInTaskbar$
 True��5   
   $��0q�6"   $��Stretch=�6-   $��Stretch=�6K   $��Microsoft Sans Serif)�6g   ��6�   Z�  � DataContext 5      .  (BackSeam.ViewModelWinColectionStanHealth  5   
    � 	Resources 5         DGridCentAl �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader	 �� Property
 �� Value��%�   &	        �� 5   
   "  6$   .�5      �� 	 � ��$
 Center=�6C   �� 5      	 � ��
 �	#FFCED7F763   5      5      .��5      � $	��520��6   $
��1000��6   $��Center=�6%   .+�5   
   �� -   5         ��Name$ 	ButtonWin��5      $��Center=�6)   $	��500��6D   $
��1000��6Q    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         Color$ 	#FF1D1504��6#      
BlurRadius$ 45Q�65      ShadowDepth$ 20Q�6E      Opacity$	 0.5Q�6V   6g   .+�5      �� -   5         ��Name$ 	BorderWin��5      ���	#FFCED7F7���	#FFCED7F7$	��500��6W   $
��1000��6d   $��0,0,0,0q�6r    �� CornerRadius$ 15,15,15,15��6�   ._�5      �  � ColumnDefinitions 5      �� 5      $
��350* �6+   �� 5       $
��630* �6+   .+�5"      � $��0��6    � RowDefinitions 5#      �� 5$      $��50 �6,   �� 5%      $
��360* �6,   .+�5'      � $��0��6   .+�5(      �� $N��H Перелік дат фіксації стану показників ��6$   $��30��6V   $��14(�6c   $��Microsoft Sans Serif)�6q   ���	#FF4607A0$��Center=�6�   $
��Left=�6�   5)      � 5*      $��1��6   .+�5,      �� ���	#FF979595$��1,1,1,1q�6=   $	��380��6W   $	��345��6d   $��Center=�6p   $	��Top=�6�   ._�5-   "   �� -   5         ��Name$! ColectionStanHealthTablGrid��5-   +    � Cursor$
 Hand��6P   ���	#FFCED7F7$	��345��5.   #   $	��380��6/   $	��Top=�6<   $��Center=�6T   $
��Auto=�6r   $��Hidden=�6�   ��5-   ^   �� *ViewPacientStanHealths+ �� SelectedItem 6�   �� *SelectedPacientStanHealth+*  $Microsoft.Xaml.Behaviors.Interaction   Triggers 5/   &   +  %Microsoft.Xaml.Behaviors.EventTrigger  50   *      	EventName$ LeftMouseUp��69   *  $Microsoft.Xaml.Behaviors.TriggerBase   Actions. 51   .   2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction     CommandParameter$  ��6�      Command 6D   ��  �� ElementName$! ColectionStanHealthTablGrid�� �� Path$( "DataContext.OnVisibleObjStanHealth�6�   52   +   ��54   &   �� 55   *   .R�56   .   �� $��0,0,0,5q�6Z   $��Center=�6k   $	��345��6�   $��20��6�   $��14(�6�   $��Microsoft Sans Serif)�6�   $
��Left=�6�   ��68   �� $ dateAnaliza�6�   57   +   ��59   &   �� 5:   *   " ��60   .�5;   .   �� 	 � ��
 �	#FFCED7F76K    �� Triggers 5<   .   �� 5=   2     �� Binding  6>   �� $ dateAnaliza�! �� Value! true6c   .a�5>   6   �� 	 � ��

 ��  �6b   5?   3   5A   +   5C   #   5D      5F      5G      � 5H      $��1��6   .+�5I      � -   5        $��GridResultInterview��5I      ���	#FFCED7F7$��Center=�6R   $��Center=�6o   $	��500��6�   $	��630��6�    5J      �� 5K   "   $	��40* �60   �� 5L   "   $
��384* �60   �� 5M   "   $	��40* �60   .+�5O      � $��0��6#   .+�5P   "   �� -   5        " ��Name$	" Dat��5P   ,   $ �� Дата фіксації��6:   ���	#FF4607A0$��Microsoft Sans Serif)�6g   $��	10,10,0,0q�6�   $��14(�6�   $��24��6�   $
��Left=�6�   $��Center=�6�   �� 5Q   "   -   5        $��
DateAnaliz��5Q   *   $��30��6�   $��14(�6�   $��0,0,0,0q�6�   ���	#FFCED7F7$��Microsoft Sans Serif)�5R   9   $��110,0,370,0q�6[   $��Center=�6p   $��Center=�6�   ��5Q   =   �� $ dateAnaliza�# �� UpdateSourceTrigger$# PropertyChanged=� 6�   �� *SelectedPacientStanHealth+5R   �   �� 5T   "   -   5        $��
Close_Grid��5T   i   $��30��6(   $��30��64   $��0��6?   $��Right=�6L   $��Center=�6|   $B��<pack://application:,,,/BackSeam;component/Images/delete2.png��5U      $
 Hand��6c    5V   &     5W   *   $ MouseLeftButtonDown��69   . 5X   .     $  ��6h    6D   �� *CloseStanHealth+6h   5Y   +   5[   #   5p      � 5q      $��1��6#   $	��380��60   $	��620��6=   .+�5r   "   �� $��Center=�6-   $	��380��6H   $	��620��6U   ���	#FFCED7F7 6x   �� *SelectedPacientStanHealth+.+�5s   &   � -   5        $ �Name$$ OpisDiagnoz��5s   D   $	��380��6+   $	��620��68    5t   *   �� 5u   .   $
��100* �6?   �� 5v   .   $
��100* �6?   �� 5w   .   $
��120* �6?   �� 5x   .   $	��50* �6?   �� 5y   .   $	��50* �6?    5{   *   �� 5|   .   $	��24* �6<   �� 5}   .   $	��24* �6<   �� 5~   .   $	��24* �6<   �� 5   .   $	��24* �6<   �� 5�   .   $	��24* �6<   �� 5�   .   $	��24* �6<   �� 5�   .   $	��24* �6<   �� 5�   .   $	��24* �6<   �� 5�   .   $	��24* �6<   �� 5�   .   $	��24* �6<   �� 5�   .   $	��24* �6<   �� 5�   .   $	��24* �6<   �� 5�   .   $	��24* �6<   �� 5�   .   $	��24* �6<   �� 5�   .   $	��24* �6<   �� 5�   .   $	��24* �6<   .+�5�   *   �� -	   5        $" Pokaznik��5�   4   $��0��6F   $��0��6S   $
��Wrap=�6c   $��14(�6w   $��  Показник��6�   $��Microsoft Sans Serif)�6�   $��90��6�   $
��Left=�6�   % �� TextAlignment$
% Left=�6�   $��24��6�   �� 5�   *   -
   5        $" PokaznikPacient��5�   4   $��0��6M   $��1��6Z   $
��Wrap=�6j   $��14(�6~   $)��#Значення показнику��6�   $��Microsoft Sans Serif)�6�   $	��140��6�   $��Center=�6�   $
% Left=�6�   $��24��6  �� 5�   *   -   5        $
" Puls��5�   4   $��1��6B   $
��Wrap=�6O   $��14(�6c   $��  Пульс��6q   $��Microsoft Sans Serif)�6�   $	��140��6�   $
��Left=�6�   $
% Left=�6�   $��24��6�   �� 5�   *   -   5        $��BoxPuls��5�   3   $��1��6�   $��Microsoft Sans Serif)�6�   & �� 
IsReadOnly$
& true��6�   ���	#FFCED7F7$	��Top=�5�   V   $��24��6n   ' �� TextWrapping$
' Wrap=�6z   $	��100��6�   $��14(�6�   $��Center=�6�   $
��Left=�6�   $��0,0,0,0q�6�   $��2��6  $��	166,0,0,0q�6  $��2��6(  ��5�   B   �� $ pulse�$# PropertyChanged=�5�   (  �� 5�   *   -   5        $" Davlenie��5�   4   $��2��6F   $
��Wrap=�6S   $��0��6h   $��24��6x   $)��#  Артеріальний тиск��6�   $��Microsoft Sans Serif)�6�   $	��140��6�   $
��Left=�6�   $��Bottom=�6�   $��14(�6  �� 5�   *   -   5        $��BoxDavlenie��5�   2   $��2��6�   $��1��6�   $��24��6�   $��14(�6�   $��Microsoft Sans Serif)�6�   $
& true��5�   :   $
' Wrap=�6O   ���	#FFCED7F7$	��Top=�6z   $��Center=�6�   $	��100��6�   $
��Left=�6�   $��0,0,0,0q�6�   ��5�   E   �� $ pressure�$# PropertyChanged=�5�   �   �� 5�   *   -   5        $" Temperatura��5�   4   $��3��6I   $'��!  Температура тіла��6V   $��Microsoft Sans Serif)�6p   $��14(�6�   $��24��6�   $	��140��6�   $
��Left=�6�   $��Center=�6�   �� 5�   *   -   5        $��BoxTemperatura��5�   2   $
' Wrap=�6�   $��1��6�   $��3��6�   $��Center=�6�   $
& true��5�   *   ���	#FFCED7F7$	��100��6S   $��24��6_   $��14(�6k   $
��Left=�6y   $��0,0,0,0q�6�   $��Microsoft Sans Serif)�6�   ��5�   H   �� $ temperature�$# PropertyChanged=�5�   �   5�   '   5�   #   5�      �� 5�      -   5        $��Podval��5�   $   $����Для перегляду деталізації опису показників натисніть на обраний рядок в списку дат, який розташовано зліва у вікні��63   $��3��6�   $��30��6�   $	��820��6�   $��14(�6�   
����  �$��Microsoft Sans Serif)�6�   $	��Top=�6  $��Center=�6/  $��Center=�6Q  $��
-300,5,0,0q�6n  5�      5�      5�      5�      5�      5�      5�      