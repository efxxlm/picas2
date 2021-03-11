import { Component, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ProjectService } from 'src/app/core/_services/project/project.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { DatePipe } from '@angular/common';
import { FollowUpDailyService } from 'src/app/core/_services/dailyFollowUp/daily-follow-up.service';
import { ActivatedRoute, Params, Router } from '@angular/router';

@Component({
  selector: 'app-ver-bitacora',
  templateUrl: './ver-bitacora.component.html',
  styleUrls: ['./ver-bitacora.component.scss']
})
export class VerBitacoraComponent implements AfterViewInit {

  proyecto: any;

  displayedColumns: string[] = [
    'fechaSeguimiento',
    'fechaValidacion',
    'productividadCodigo',
    'estadoCodigo',
    'seguimientoDiarioId'
  ];
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private followUpDailyService: FollowUpDailyService,
    private route: ActivatedRoute,
    private router: Router,
    public dialog: MatDialog,
  ) 
  {
    if (this.router.getCurrentNavigation().extras.replaceUrl) {
      this.router.navigateByUrl('/registroSeguimientoDiario');
      return;
    };

    if (this.router.getCurrentNavigation().extras.state)
      this.proyecto = this.router.getCurrentNavigation().extras.state.proyecto;

    console.log( this.proyecto )
   }

  ngAfterViewInit() {
    this.route.params.subscribe((params: Params) => {
      this.followUpDailyService.getDailyFollowUpByContratacionProyectoId( params.id )
      .subscribe(listaSeguimientos => {

        // no debe mostrar los seguimeintos que no se hayna enviado para verificacion
        listaSeguimientos = listaSeguimientos.filter( r => r.estadoCodigo != "1" )

        this.dataSource = new MatTableDataSource( listaSeguimientos );

        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
        this.paginator._intl.nextPageLabel = 'Siguiente';
        this.paginator._intl.getRangeLabel = (page, pageSize, length) => (page + 1).toString() + ' de ' + length.toString();
        this.paginator._intl.previousPageLabel = 'Anterior';

      });

    });
    
    
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  verDetalle( seguimiento ){
  this.router.navigate( [ '/registroSeguimientoDiario/verDetalle', seguimiento.seguimientoDiarioId ? seguimiento.seguimientoDiarioId : 0 ], { state: { proyecto: seguimiento.contratacionProyecto.proyecto.infoProyecto } } )
    
  }

  Editar( seguimiento ){
    this.router.navigate( [ '/registroSeguimientoDiario/registrarSeguimiento', seguimiento.seguimientoDiarioId ? seguimiento.seguimientoDiarioId : 0 ], { state: { proyecto: seguimiento.contratacionProyecto.proyecto.infoProyecto } } )
  }

  Enviar( id ){
    this.followUpDailyService.sendToSupervisionSupport( id )
      .subscribe( respuesta => {
        this.openDialog( '', respuesta.message )
        if ( respuesta.code == "200" )
          this.ngAfterViewInit();

      })
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }
}
