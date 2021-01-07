import { Component, Input, OnInit, Output, ViewChild, EventEmitter } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Observable } from 'rxjs';
import { DialogObservacionesFlujoRecursosComponent } from '../dialog-observaciones-flujo-recursos/dialog-observaciones-flujo-recursos.component';
import { FaseUnoConstruccionService } from '../../../../core/_services/faseUnoConstruccion/fase-uno-construccion.service';
import { DialogObservacionesProgramacionComponent } from '../dialog-observaciones-programacion/dialog-observaciones-programacion.component';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { CommonService } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-tabla-inversion-recursos',
  templateUrl: './tabla-inversion-recursos.component.html',
  styleUrls: ['./tabla-inversion-recursos.component.scss']
})
export class TablaInversionRecursosComponent implements OnInit {

  dataSource = new MatTableDataSource();
  @Input() contratoConstruccionId: number;
  @Input() observacionDevolucionFlujoInversion: any;
  @Input() proyectoId: number;
  @Input() contratoId: number;

  @Output() tieneRegistros = new EventEmitter();
  @Output() realizoObservacion = new EventEmitter();
  @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
  @ViewChild( MatSort, { static: true } ) sort          : MatSort;
  displayedColumns: string[] = [ 
    'fechaCreacion',
    'cantidadRegistros',
    'cantidadRegistrosValidos',
    'cantidadRegistrosInvalidos',
    'estadoCargue',
    'gestion'
  ];

  constructor ( private dialog: MatDialog,
                private faseUnoConstruccionSvc: FaseUnoConstruccionService,
                private commonService: CommonService 
              )
  {
  }

  ngOnInit(): void {
    this.getData();
  }

  getData () {
    if ( this.contratoConstruccionId !== 0 ) {
      this.faseUnoConstruccionSvc.getLoadInvestmentFlowGrid( this.contratoConstruccionId )
      .subscribe( ( response: any[] ) => {
        if ( response.length === 0 ) {
          this.tieneRegistros.emit( false );
          return;
        }
        this.tieneRegistros.emit( true );
        this.dataSource                        = new MatTableDataSource( response );
        this.dataSource.paginator              = this.paginator;
        this.dataSource.sort                   = this.sort;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
      } )
    };
  };

  openDialog (modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
  };

  applyFilter ( event: Event ) {
    const filterValue      = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };

  getSemaforo ( observacion: string ) {
    if ( observacion !== null ) {
      return 'completo';
    } else {
      return 'sin-diligenciar';
    }
  }
  
  addObservaciones( pArchivoCargueId: number, estadoCargue: string, fechaCreacion, observaciones?: string ){
    const dialogCargarProgramacion = this.dialog.open( DialogObservacionesProgramacionComponent, {
      width: '75em',
      data: { pArchivoCargueId, observaciones, estadoCargue, fechaCreacion, 
              contratoId: this.contratoId, proyectoId: this.proyectoId,
              esFlujoInversion: true }
    });
    dialogCargarProgramacion.afterClosed()
      .subscribe( response => {
        if ( response.realizoObservacion ) {
          this.dataSource = new MatTableDataSource();
          this.getData();
        };
      } )
  };

  deleteArchivoCargue( pArchivoCargueId: number ){
    this.faseUnoConstruccionSvc.deleteArchivoCargue( pArchivoCargueId, this.contratoConstruccionId, true )
      .subscribe(
        response => {
          this.openDialog( '', response.message );
          this.dataSource = new MatTableDataSource();
          this.getData();
          this.realizoObservacion.emit( true );
        },
        err => this.openDialog( '', err.message )
      )
  };

  descargar( pArchivoCargueId: number ) {
    this.commonService.getFileById( pArchivoCargueId )
      .subscribe(respuesta => {
        let documento = "FlujoInversion.xlsx";
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
