   M S B A M L   `   `   `  ����  UP    EKZ.Views.MainView   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	     � Title$  MainView��5   	   $	��600��6   $
��1000��6'   .��5      � $��10q�6   .+�5	   
   ��  �� Orientation$ 
Horizontal=�6   $	��Top=�6.   $��0,0,0,0q�6F   .+�5
      �� $	��150��6   $��0,0,10,0q�6#   ��5      �� *FilterOptions+ �� SelectedItem 5      �� *SelectedFilter+�� 5      $	��200��6   $��0,0,10,0q�6"   ��5      �� *
SearchText+ �� UpdateSourceTrigger$ PropertyChanged=��� 5      $��Применить��6   $	��100��6)   $��0,0,10,0q�65   ��6G   �� *ApplyFilterCommand+�� 5      $��Сбросить��6   $	��100��6(   $��0,0,10,0q�64   ��6F   �� *ResetFilterCommand+�� 5      $��Отменить��6   $	��100��6(   ��64   �� *CancelFilterCommand+�� 5      $��
CarsReport��6   $	��100��6*   $��80, 0 , 0, 0q�66   ��6L   �� *GoCarsReportCommand+�� 5      $��RepairSummary��6   $	��100��6-   $��10, 0 , 0, 0q�69   ��6O   �� *GoRepairSummaryCommand+5      &   System.Windows.Controls.DataGrid  5   
   $��0, 30, 0 , 25q�5         AutoGenerateColumns$
 True��5         
IsReadOnly$
 True��5      $��Stretch=�5      $��Stretch=�5      ��5      �� *FilteredData+5      �� 5   
   $1��+Редактировать клиентов��6   $��Bottom=�5!      $
��Left=�5"      $��10,0,0,0q�5#      $	��200��6+   ��5       �� *
AddCommand+5#   +   �� 5%   
   $��10,0,0,0q�6(   $	��200��6:   $��20��6F   $��Bottom=�6R   $��Center=�5&      
���   ���5%      �� *Role+.��5&   B   6E   �� 5'      $"��Сформировать PDF��6   $��Bottom=�5)      $��Right=�5*      $��10,0,0,0q�5+      $	��200��6+   ��5(      �� *GeneratePdfCommand+5+   +   5-      5.      