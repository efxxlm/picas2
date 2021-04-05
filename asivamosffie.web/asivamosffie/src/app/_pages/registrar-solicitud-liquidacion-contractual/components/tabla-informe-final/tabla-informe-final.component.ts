import { Component, ViewChild, OnInit, Input } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { RegisterContractualLiquidationRequestService } from 'src/app/core/_services/registerContractualLiquidationRequest/register-contractual-liquidation-request.service';

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
  @Input() contratacionProyectoId: number;

  datosTabla = [];

  @ViewChild(MatSort) sort: MatSort;
  constructor(
    private registerContractualLiquidationRequestService: RegisterContractualLiquidationRequestService
  ) { 
  }

  ngOnInit(): void {
    this.gridInformeFinal(this.contratacionProyectoId);
  }

  gridInformeFinal(contratacionProyectoId: number) {
    this.registerContractualLiquidationRequestService.gridInformeFinal(contratacionProyectoId).subscribe(report => {
      if(report != null){
        report.forEach(element => {
          this.datosTabla.push({
            fechaEnvio : element.fechaEnvio.split('T')[0].split('-').reverse().join('/'),
            fechaAprobacion : element.fechaAprobacion.split('T')[0].split('-').reverse().join('/'),
            llaveMen: element.llaveMen,
            tipoIntervencion: element.tipoIntervencion,
            institucionEducativa: element.institucionEducativa,
            sede: element.sede,
            estadoValidacion: element.estadoValidacion,
            contratacionProyectoId: contratacionProyectoId,
            proyectoId: element.proyectoId
          });
        })
      }
      this.dataSource.data = this.datosTabla;
    });
  }

}