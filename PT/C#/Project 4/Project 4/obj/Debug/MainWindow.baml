   M S B A M L   `   `   `  ����D  @Project 4, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:Project_4	Project_4  5   	       Project_4.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:Project_4     5   	     � Title$  
MainWindow��5   	   $	��759��6   $	��506��6(   $��0q�65   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $
��460* �6   �� 5      $
��291* �6   .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ dataGrid��5      $
��Left=�5      $��0,40,0,0q�5      $	��Top=�5      $	��427��5      $	��450��5         MinColumnWidth$	 150Q�6      ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$	 150 64      AutoGenerateColumns .  ��5      ��  �� Source$ MyCars��   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Model��6%   1  +System.Windows.Controls.DataGridBoundColumn	   Binding	 64   �� *Model+  5      $ Engine��6%   	 65   �� *Motor+  5      $
 Year��6%   	 63   �� *Year+5      �� 5   
   -   5        
 ��Name$
 textBox��5      $��1��6#   $
��Left=�63   $��
0,125,0,10q�6N    �� TextWrapping$
 Wrap=�6b   $	��281��6v   �� 5   
   -   5         ��Name$ button1��5      $��Task 1��6"   $��1��63   $
��Left=�6C   $��	0,100,0,0q�6^   $	��Top=�6q   $	��136��6�   6�   �� 5   
   -   5        $ button2��5      $��Task 2��6"   $��1��63   $
��Left=�6C   $��145,100,0,0q�6^   $	��Top=�6s   $	��136��6�   6�   �� 5   
   -   5        $ deleteButton��5      $��Delete��6'   $��1��68   $
��Left=�6H   $��	10,10,0,0q�6c   $	��Top=�6v   $��75��6�   6�   5      5       