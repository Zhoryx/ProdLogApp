   M S B A M L   `   `   `  ����E  AProdLogApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS.clr-namespace:ProdLogApp.Views.Designs.Prompts ProdLogApp.Views.Designs.Prompts  5   	   4    .ProdLogApp.Views.Designs.Prompts.PromptProduct   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression.blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   @local.clr-namespace:ProdLogApp.Views.Designs.Prompts     5   	     � Title$  Seleccionar Producto��5   	   $	��500��6&   $	��850��63    � WindowStartupLocation$ CenterScreen=�5	   	    � 
ResizeMode$ NoResize=�5
   	   ���#0864AC � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ /Views/Designs/Style.xaml?�6%   %    5      .��5      � $��20q�6   .+�5   
   ��  �� Orientation$ Vertical=�6   $
��Left=�6,   $��0,0,0,10q�6G   .+�5      �� $��Buscar por nombre:��6   $��14(�61   
�������$��0,0,0,5q�6R   �� 5      -   5         ��Name$ 	SearchBox��5      $	��250��6)   $��40��65      ModernTextBoxStyle#��[  6A   �� 5      $��Filtrar por categoría:��6   $��14(�65   
�������$��0,10,0,5q�6V   �� 5      -   5         ��Name$ CategoryFilter��5      $	��250��6/   $��40��6;     ModernComboBoxStyle#��[ 6G   .@�5      �� $��Todas las categorías��6   	 �� 
IsSelected$
	 True��6>   5      5      �� 5"   
     ShadowBorderStyle#��[ 6   
 �� Padding$
 10q�6<   $��
280,0,0,80q�6I   $
��Left=�6]   $	��530��6x   ._�5#      �� -   5         ��Name$ ProductList��5#      $	��320��6,    �� SelectionMode$ Single=�69    �� View 5$      �� 5%      .P�5&      �� $��Nombre del Producto��6)    �� Width$	 250��6v    �� DisplayMemberBinding 6F   �� *NombreProducto+6v   �� 5'      $��
Categoría��6)   $	 200��6o    6<   �� *CategoriaProducto+6o   5(      5*      5+      �� 5.   
   $ 
Horizontal=�6   $��Center=�6.   $��Bottom=�6K   $��0,0,0,20q�6f   .+�5/      �� -   5         ��Name$ ConfirmButton��5/      $��	Confirmar��6,     ConfirmButtonStyle#��[ 6@   $	��120��6l   �� 50      -   5        $ CancelButton��50      $��Cancelar��6+     CancelButtonStyle#��[ 6>   $	��120��6i   $��10,0,0,0q�6u   51      52      53      