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
    'contratacionId'
  ];

  dataSource = new MatTableDataSource(this.ELEMENT_DATA);
  datosTabla = [];

  @Input() contratacionId: number;
  @Input() esVerDetalle: boolean;
  @Output() semaforoInformeFinal = new EventEmitter<string>();
  listaMenu: ListaMenuSolicitudLiquidacion = ListaMenuSolicitudLiquidacionId;
  total: number = 0;
  totalCompleto: number = 0;
  totalIncompleto: number = 0;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private registerContractualLiquidationRequestService: RegisterContractualLiquidationRequestService
  ) { 
  }

  ngOnInit(): void {
    this.gridInformeFinal(this.contratacionId);
  }

  gridInformeFinal(contratacionId: number) {
    this.registerContractualLiquidationRequestService.gridInformeFinal(contratacionId, this.listaMenu.aprobarSolicitudLiquidacionContratacion).subscribe(report => {
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
            contratacionId: contratacionId,
            proyectoId: element.proyectoId
          });
        })
      }
      this.dataSource.data = this.datosTabla;
      this.total = this.datosTabla.length;
      this.datosTabla.forEach(element => {
        if(element.registroCompleto === 'Completo')
          this.totalCompleto++;
        if(element.registroCompleto === 'Incompleto')
          this.totalIncompleto++;
      });
      if(this.total <= 0){
        this.semaforoInformeFinal.emit(null);
      }
      else if(this.totalCompleto >= this.total){
        this.semaforoInformeFinal.emit('Completo');
      }else if(this.totalIncompleto >= this.total){
        this.semaforoInformeFinal.emit(null);
      }else{
        this.semaforoInformeFinal.emit('Incompleto');
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