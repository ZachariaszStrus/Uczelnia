   M S B A M L   `   `   `  ����D  @Project 2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:Project_2	Project_2  5   	   "    Project_2.FileCreationWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:Project_2     5   	     � Title$  FileCreationWindow��5   	   $	��307��6$   $	��214��61   .��5	      � .+�5
   
   	� -   5         	�Name$ fileRadioButton��5
       	� 	GroupName$ 	fileOrDir��6/   $
��file��6E    d� 	IsChecked$
 TrueY�6T   $��10,63,138,156q�6e   	� 5   
   -   5        $ directoryRadioButton��5      $ 	fileOrDir��64   $��	directory��6J   $��10,41,101,178q�6^   �� 5   
   -   5         ��Name$ label��5      $
��Name��6   $
��Left=�6.   $��	10,10,0,0q�6I   $	��Top=�6\   $��45��6t   �� 5   
   -   5         ��Name$ fileNameTextBox��5      $
��Left=�6+   $��20��6F   $��	65,16,0,0q�6R   $��
��6e   $	��Top=�6w   $	��121��6�    �� AcceptsReturn . 6�   �� 5   
   -   5         ��Name$ groupBox��5      $��
Attributes��5      $
��Left=�5      $��	10,85,0,0q�5      $	��Top=�5      $	��110��5      $	��176��6    .��5      � $��0,0,0,-2q�6   .+�5      �� -   5         ��Name$ readOnlyCheckBox��5      $��	Read Only��65   $
��Left=�6I   $��	10,10,0,0q�6d   $	��Top=�6w   �� 5      -   5        $ archiveCheckBox��5      $��Archive��64   $
��Left=�6F   $��	10,30,0,0q�6a   $	��Top=�6t   �� 5      -   5        $ hiddenCheckBox��5      $��Hidden��63   $
��Left=�6D   $��	10,50,0,0q�6_   $	��Top=�6r   �� 5      -	   5        $ systemCheckBox��5      $��System��63   $
��Left=�6D   $��	10,70,0,0q�6_   $	��Top=�6r   5      5      �� 5   
   -
   5        	 ��Name$	 cancelButton��5      $��Cancel��6'   $
��Left=�68   $��120,210,0,0q�6S   $	��Top=�6h   $��66��6�   6�   �� 5   
   -   5        $	 okButton��5      $��OK��6#   $
��Left=�60   $��
66,210,0,0q�6K   $	��Top=�6_   $��49��6w   6�   �� 5   
   -   5        $ messageLabel��5      $�� ��5       $
��Left=�5!      $��
10,235,0,0q�5"      $	��Top=�5#      $	��176��5$      $��31��6   5%      5&      