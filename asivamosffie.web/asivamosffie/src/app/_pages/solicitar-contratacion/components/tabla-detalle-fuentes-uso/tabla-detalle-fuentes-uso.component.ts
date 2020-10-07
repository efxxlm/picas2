import { Component, Input, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-detalle-fuentes-uso',
  templateUrl: './tabla-detalle-fuentes-uso.component.html',
  styleUrls: ['./tabla-detalle-fuentes-uso.component.scss']
})
export class TablaDetalleFuentesUsoComponent implements OnInit {

  @Input() contratacionProyecto: any;
  dataSource = new MatTableDataSource();
  dataTable: any[] = [];
  displayedColumns: string[] = [     
    'componente',
    'fase',
    'tipoUso',
    'valorUso'
  ]

  constructor() { }

  ngOnInit(): void {
    setTimeout(() => {
      if ( this.contratacionProyecto.dataAportantes.length > 0 ) {
        this.dataTable = this.contratacionProyecto.dataAportantes;
        this.dataSource = new MatTableDataSource ( this.dataTable );
      }
    }, 1000);
  };

}
