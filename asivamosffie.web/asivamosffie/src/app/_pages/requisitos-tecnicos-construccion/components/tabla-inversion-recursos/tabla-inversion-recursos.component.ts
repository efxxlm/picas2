import { Component, Input, OnInit, Output, ViewChild, EventEmitter } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Observable } from 'rxjs';
import { DialogObservacionesFlujoRecursosComponent } from '../dialog-observaciones-flujo-recursos/dialog-observaciones-flujo-recursos.component';
import { FaseUnoConstruccionService } from '../../../../core/_services/faseUnoConstruccion/fase-uno-construccion.service';

@Component({
  selector: 'app-tabla-inversion-recursos',
  templateUrl: './tabla-inversion-recursos.component.html',
  styleUrls: ['./tabla-inversion-recursos.component.scss']
})
export class TablaInversionRecursosComponent implements OnInit {

  dataSource = new MatTableDataSource();
  @Input() contratoConstruccionId: number;
  @Output() tieneRegistros = new EventEmitter();
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
                private faseUnoConstruccionSvc: FaseUnoConstruccionService )
  {
  }

  ngOnInit(): void {
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
    }
  }

  applyFilter ( event: Event ) {
    const filterValue      = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };
  
  addObservaciones(){
   const dialogCargarProgramacion = this.dialog.open( DialogObservacionesFlujoRecursosComponent, {
      width: '75em'
    });

    dialogCargarProgramacion.afterClosed().subscribe( resp => {
      console.log( resp );
    } );
  }
}
