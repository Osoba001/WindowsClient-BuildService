   M S B A M L   `   `   `  ����X  TWindowsClientBuildSelfService, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullW0clr-namespace:WindowsClientBuildSelfService.Main"WindowsClientBuildSelfService.Main  5   	   yAclr-namespace:WindowsClientBuildSelfService.Share.ValueConverters3WindowsClientBuildSelfService.Share.ValueConverters  5   	   ;    5WindowsClientBuildSelfService.Main.StartupWindowsView   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Blocal0clr-namespace:WindowsClientBuildSelfService.Main     5   	   W	converterAclr-namespace:WindowsClientBuildSelfService.Share.ValueConverters     5   	     � Title$  SEPAL BUILDS SELF-SERVICE��5	   	   $	��800��6+   $	��700��68   $	��700��6D   $	��700��6S    � WindowStartupLocation$ CenterScreen=�6b    � 	Resources 5
      P   JWindowsClientBuildSelfService.Share.ValueConverters.StatusToBrushConverter   StatusToBrushConverter��%   &	          5   
   6+   5      .��5      �  � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      .+�5   
   �� $��0��6   $��SEPAL BUILDS SELF-SERVICE��6!   
���   �$��Center=�6U   $	��Top=�6r   $
��Bold"�6�   $��25(�6�    �� Padding$ 15q�6�   6�   �� 5   
   $��1��6   $
��Auto=�6$   $	��600��6G   $	��650��6T   .��5      �� $��1��6   ��6)   �� *PullRequest+��5      �� 5      .R�5      �� 
���   �$��1q�65    �� CornerRadius$ 5��6I    �� Padding$ 10q�6Z   $��12q�6g   ._�5      �  5   "   �� 5   &   $
��auto �64    � ColumnDefinitions 5   "   �� 5    &   �� 5!   &   $
��auto �67   .+�5#   "   �� $��0��6-   .+�5$   &   �� $
��Bold"�6i   $��0,0,0,5q�6{   $��15(�6�   ��60   �� *Title+ �� StringFormat$ CCL Project - {0}��6�   �� 5%   &   	 �� Orientation$	 
Horizontal=�61   .+�5&   *   �� $��Author: ��64   6F   �� 5'   *   ��64   �� *Author+5)   '   �� 5*   &   $	 
Horizontal=�61   .+�5+   *   �� $��Status: ��64   6F   �� 5,   *   $��12(�6�   $
��Bold"�6�   ��64   �� *Status+��6L   �� *Status+
 �� 	Converter#
 [  5.   +   5/   '   �� 50   &   $	 
Horizontal=�61   .+�51   *   �  � Minimum$ 0Q�66    � Maximum$	 100Q�6B   $��5��6P   $	��150��6[    � Value 6g   �� *Release.DownloadProgress+ �� UpdateSourceTrigger$ PropertyChanged=��� 52   *   $
��Bold"�6s   $��10,0,0,0q�6�   ��64   �� *Release.DownloadProgress+$
 {0}%��6�   53   '   �� 54   &   $	 
Horizontal=�61   .+�55   *   �� $
��Bold"�6i   $��12(�6{   $��0,0,0,0q�6�   ��64   �� *	CreatedAt+$ dd MMMM yyyy��6�   56   '   57   #   ]� 58   "   $��1��6,   .+�5:   &   �� $��Launch��6[   $��30��6l   $��75��6x   $��12(�6�   $
��Bold"�6�   
���������6-   �� *Release.DownloadPRRelease+��5;   *   �� 5<   .    �� 
TargetType" ��64   .�5=   2   ��  �� Property �@J�Opacity �� Value$	 1.0Q�6L   �� 5>   2    � ��
 ��  �6O   �� 5?   2    � �� 5@   6   �� 5A   :    �� 
TargetType" ��6J   .R�5B   >   � .+�5C   B   �� $ 5��6I   #��z��6Z   �� 5D   B   $��Center=�6S   $��Center=�6p   5E   ?   5F   ;   5H   3    �� Triggers 5I   2   P� 5J   6    P� Property � {� P� Value$ False��6S   . �5K   :   ��  �@J�Opacity$	 0.5Q�6T   5L   7   P� 5M   6    � ��$
 True��6S   . �5N   :   ��  � ��
 ���_�6W   �� 5O   :    � ��
 �����6W   5P   7   5R   /   5T   '   5U   #   5V      5W      5X      5Z      5[      �� 5\   
   $��2��6   $��An Update is Available��6!   $��Right=�6@   $	��Top=�6\   $��13(�6t   $ 13q�6�   $
��Bold"�6�   6�   �� 5]   
   $��3��6   $��12(�6!   $ 10q�6/   $#��CypherCrescent Limited ©2023��6<   
����  �$��Center=�6r   $��Bottom=�6�   5^      5_      