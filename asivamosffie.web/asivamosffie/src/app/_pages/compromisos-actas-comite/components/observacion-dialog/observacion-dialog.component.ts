import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-observacion-dialog',
  templateUrl: './observacion-dialog.component.html',
  styleUrls: ['./observacion-dialog.component.scss']
})
export class ObservacionDialogComponent implements OnInit {

  displayedColumns: string[] = [ 'fecha', 'observacion' ];
  dataSource = new MatTableDataSource();
  @ViewChild( MatSort, { static: true } ) sort: MatSort;
  
  constructor ( @Inject(MAT_DIALOG_DATA) public data ) { }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource( this.data.elemento.sesionSolicitudObservacionProyecto );
    this.dataSource.sort = this.sort;
  }

  textoLimpioMessage (texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio;
    };
  };

}
