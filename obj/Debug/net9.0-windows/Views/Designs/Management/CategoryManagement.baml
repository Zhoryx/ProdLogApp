   M S B A M L   `   `   `  ����  
ProdLogApp)    #ProdLogApp.Views.CategoryManagement   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Menu - Producciones��5   	   $	��720��5   	   $
��1080��5   	    � WindowStartupLocation$ CenterScreen=�5   	   ���#0864AC � 
ResizeMode$ NoResize=�5	   	    � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ /Views/Designs/Style.xaml?�6%   %    5      .��5      � $
��Left=�6   $
��1080��6&    � ColumnDefinitions 5   
   �� 5      $��3* �6   �� 5      $��2* �6   .+�5   
   �� ���#E6F0FA �� Padding$ 30q�6&   $��	10,0,10,0q�63   $��Center=�6F   $	��586��6a   ._�5      �� -   5         ��Name$ CategoryList��5      	 �� SelectionMode$	 Single=�6-   
 �� View
 5      �� 5      .P�5      ��  �� Width$ 80��6)    �� DisplayMemberBinding 64   �� *
CategoryId+��5      �� 5    "   -   5        $��Código��5    7   
 � Tag$ Id��6k   5"      �� 5$      $	 400��6)    65   �� *Nombre+��5%      �� 5&   "   -   5        $��Nombre de Categoría��5&   7   $ Nombre��6x   5(      �� 5*      $	 100��6)    65   �� *EstadoActivo+��5+      �� 5,   "   -   5        $��Activo��5,   7   $ EstadoActivo��6k   5.      5/      52      53      � 56   
   $��1��6   $��Center=�6   $��Center=�6:   $��0,30,0,0q�6W    � RowDefinitions 57      �� 58      $
��Auto �6    �� 59      $
��Auto �6    �� 5:      $
��Auto �6    �� 5;      $
��Auto �6    .+�5>      �� -   5           ModernButtonStyle#��[  5>      $��Agregar��6@   $��50��6R   $	��250��6^   $��0��6�   $��10q�6�   �� 5?      -   5        #��[  5?      $��	Modificar��6@   $��50��6T   $	��250��6`   $��1��6�   $��10q�6�   �� 5@      -   5        #��[  5@      $��Activar/Desactivar��6@   $��50��6]   $	��250��6i   $��2��6�   $��10q�6�   �� 5A      -   5        #��[  5A      $��Volver��6@   $��50��6Q   $	��250��6]   $��3��6�   $��10q�6�   5B      5C      5D      