   M S B A M L   `   `   `  ����D  @Project 2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:Project_2	Project_2  5   	       Project_2.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:Project_2     5   	     � Title$  
MainWindow��5   	   $	��600��6   $	��500��6)    � 
ResizeMode$ NoResize=�65   .��5	      � .+�5
   
   o� -   5         o�Name$
 menu��5
      $
��Left=�5      $	��Top=�5      $	��494��5      .@�5      m� $
��File��6   .@�5      m� -   5         m�Name$ openMenuItem��5      $
��Open��61   6?   m� 5      -   5        $ exitMenuItem��5      $
��Exit��61   6?   5      5      R� 5   
   -   5         R�Name$ treeView��5      $
��Left=�5      $	��367��5      $��0,23,0,0q�6    $	��Top=�5      $	��484��5      ��5      �� 5       �� DataType   Project_2.FileTree"  5       �� ItemsSource 5      �� *	FileSet+ �� UpdateSourceTrigger$ PropertyChanged=�.R�5      ��  ��Name$ contextMenu��6!   	 � ContextMenu	 5      �� 5      -   5        .@�5   "   m� $ deleteContextMenuItem��6+   -   5        $��Delete��5   J   5    +   m� 5!   "   $ addContextMenuItem��6+   -   5        $	��Add��5!   G   5"   #   5#      .+�5%      �� ��6$   �� 
 �� Path$
 FileName�5&      5'      5*      �� 5+   
   -   5         ��Name$ 	statusBar��5+      $
��Left=�6'   $��21��5,      $��	0,540,0,0q�5-      $	��Top=�5.      $	��484��5/      .@�51      �� -	   5         ��Name$ rahsTextBlock��51      52      �� 53   
   -
   5         ��Name$ textBox��53      $
��Left=�54      $	��140��55      $��	0,395,0,0q�56       �� TextWrapping$
 Wrap=�57      $��TextBox��58      $	��Top=�59      $	��484��5:      5;      5<      