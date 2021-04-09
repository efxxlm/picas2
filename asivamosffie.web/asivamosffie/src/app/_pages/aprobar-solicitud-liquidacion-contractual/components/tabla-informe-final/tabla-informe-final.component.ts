import { Component, AfterViewInit, ViewChild, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { RegisterContractualLiquidationRequestService } from 'src/app/core/_services/registerContractualLiquidationRequest/register-contractual-liquidation-request.service';
import { ListaMenuSolicitudLiquidacion, ListaMenuSolicitudLiquidacionId } from 'src/app/_interfaces/estados-solicitud-liquidacion-contractual';

@Component({
  selector: 'app-tabla-informe-final',
  templateUrl: './tabla-informe-final.component.html',
  styleUrls: ['./tabla-informe-final.component.scss']
})
export class TablaInformeFinalComponent implements OnInit {

  ELEMENT_DATA: any[] = [];
  displayedColumns: string[] = [
    'fechaEnvio',
    'fechaAprobacion',
    'llaveMen',
    'tipoIntervencion',
    'institucionEducativa',
    'sede',
    'estadoValidacion',
    'contratacionProyectoId'
  ];

  dataSource = new MatTableDataSource(this.ELEMENT_DATA);
  datosTabla = [];

  @Input() contratacionProyectoId: number;
  @Input() esVerDetalle: boolean;
  @Output() semaforoInformeFinal = new EventEmitter<string>();
  listaMenu: ListaMenuSolicitudLiquidacion = ListaMenuSolicitudLiquidacionId;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private registerContractualLiquidationRequestService: RegisterContractualLiquidationRequestService
  ) { 
  }

  ngOnInit(): void {
    this.gridInformeFinal(this.contratacionProyectoId);
  }

  gridInformeFinal(contratacionProyectoId: number) {
    this.registerContractualLiquidationRequestService.gridInformeFinal(contratacionProyectoId, this.listaMenu.aprobarSolicitudLiquidacionContratacion).subscribe(report => {
      if(report != null){
        report.forEach(element => {
          this.datosTabla.push({
            fechaEnvio : element.fechaEnvio.split('T')[0].split('-').reverse().join('/'),
            fechaAprobacion : element.fechaAprobacion.split('T')[0].split('-').reverse().join('/'),
            llaveMen: element.llaveMen,
            tipoIntervencion: element.tipoIntervencion,
            institucionEducativa: element.institucionEducativa,
            sede: element.sede,
            estadoValidacion: element.registroCompleto ? 'Con validación' : 'Sin validación',
            registroCompleto: element.registroCompleto ? 'Completo' : 'Incompleto',
            contratacionProyectoId: contratacionProyectoId,
            proyectoId: element.proyectoId
          });
        })
      }
      this.dataSource.data = this.datosTabla;
      if(this.datosTabla.length > 0){
        this.semaforoInformeFinal.emit(this.datosTabla[0].registroCompleto);
      }
    });
  }


  // ngAfterViewInit() {
  //   this.dataSource.sort = this.sort;
  //   this.dataSource.paginator = this.paginator;
  //   this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
  //   this.paginator._intl.nextPageLabel = 'Siguiente';
  //   this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
  //     if (length === 0 || pageSize === 0) {
  //       return '0 de ' + length;
  //     }
  //     length = Math.max(length, 0);
  //     const startIndex = page * pageSize;
  //     // If the start index exceeds the list length, do not try and fix the end index to the end.
  //     const endIndex = startIndex < length ?
  //       Math.min(startIndex + pageSize, length) :
  //       startIndex + pageSize;
  //     return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
  //   };
  //   this.paginator._intl.previousPageLabel = 'Anterior';
  // }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

}