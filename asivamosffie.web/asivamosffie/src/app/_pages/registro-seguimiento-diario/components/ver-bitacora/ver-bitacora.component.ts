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


export interface SeguimientoDiario {
  id: string;
  fechaRegistro: string;
  fechaValidacion: string;
  productividad: string;
  estadoSeguimiento: string;
}

const ELEMENT_DATA: SeguimientoDiario[] = [
  {
    id: '1',
    fechaRegistro: '05/07/2020',
    fechaValidacion: 'Sin seguimiento ',
    productividad: 'Alta',
    estadoSeguimiento: 'Seguimiento diario enviado'
  }
];

@Component({
  selector: 'app-ver-bitacora',
  templateUrl: './ver-bitacora.component.html',
  styleUrls: ['./ver-bitacora.component.scss']
})
export class VerBitacoraComponent implements AfterViewInit {

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
  ) 
  { }

  ngAfterViewInit() {
    this.route.params.subscribe((params: Params) => {
      //this.seguimientoId = params.id;
      this.followUpDailyService.getDailyFollowUpByContratacionProyectoId( params.id )
      //this.followUpDailyService.getDailyFollowUpById(  )
      .subscribe(respuesta => {
        this.dataSource = new MatTableDataSource(respuesta);

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
}
