import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { FaseUnoConstruccionService } from 'src/app/core/_services/faseUnoConstruccion/fase-uno-construccion.service';
import { DialogObservacionesProgramacionComponent } from '../dialog-observaciones-programacion/dialog-observaciones-programacion.component';

@Component({
  selector: 'app-detalle-tabla-programacion-obra',
  templateUrl: './detalle-tabla-programacion-obra.component.html',
  styleUrls: ['./detalle-tabla-programacion-obra.component.scss']
})
export class DetalleTablaProgramacionObraComponent implements OnInit {

  dataSource = new MatTableDataSource();
  @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
  @ViewChild( MatSort, { static: true } ) sort          : MatSort;
  displayedColumns: string[] = [ 
    'fechaCreacion',
    'cantidadRegistros',
    'cantidadRegistrosValidos',
    'cantidadRegistrosInvalidos',
    
    'gestion'
  ];
  

  @Input() contratoConstruccionId: number;

  constructor(
              private dialog: MatDialog,
              private faseUnoConstruccionSvc: FaseUnoConstruccionService,
              private commonService: CommonService, 
              ) {
  }

  ngOnInit(): void {
    this.getData();
  };

  getData () {
    console.log( this.contratoConstruccionId )
    if ( this.contratoConstruccionId !== 0 ) {
      this.faseUnoConstruccionSvc.getLoadProgrammingGrid( this.contratoConstruccionId )
      .subscribe( ( response: any[] ) => {
        this.dataSource                        = new MatTableDataSource( response );
        this.dataSource.paginator              = this.paginator;
        this.dataSource.sort                   = this.sort;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
      } );
    }
  };

  applyFilter ( event: Event ) {
    const filterValue      = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };
  
  addObservaciones( pArchivoCargueId: number, estadoCargue: string, fechaCreacion, observaciones?: string  ){
    const dialogCargarProgramacion = this.dialog.open( DialogObservacionesProgramacionComponent, {
      width: '75em',
      data: { pArchivoCargueId, observaciones, estadoCargue, fechaCreacion, esFlujoInversion: false, ocultarBoton:true }
    });
    dialogCargarProgramacion.afterClosed()
      .subscribe( response => {
        if ( response.realizoObservacion ) {
          this.dataSource = new MatTableDataSource();
          this.getData();
        };
      } )
  };

  descargar( pArchivoCargueId: number ) {
    this.commonService.getFileById( pArchivoCargueId )
      .subscribe(respuesta => {
        let documento = "ProgramacionObra.xlsx";
        var text = documento,
          blob = new Blob([respuesta], { type: 'application/octet-stream' }),
          anchor = document.createElement('a');
        anchor.download = documento;
        anchor.href = window.URL.createObjectURL(blob);
        anchor.dataset.downloadurl = ['application/octet-stream', anchor.download, anchor.href].join(':');
        anchor.click();
      });
  }

}
