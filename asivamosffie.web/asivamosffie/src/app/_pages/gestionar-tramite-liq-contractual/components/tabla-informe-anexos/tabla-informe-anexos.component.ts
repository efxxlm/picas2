import { Component, AfterViewInit, ViewChild, OnInit, Input } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { RegisterContractualLiquidationRequestService } from 'src/app/core/_services/registerContractualLiquidationRequest/register-contractual-liquidation-request.service';

@Component({
  selector: 'app-tabla-informe-anexos',
  templateUrl: './tabla-informe-anexos.component.html',
  styleUrls: ['./tabla-informe-anexos.component.scss']
})
export class TablaInformeAnexosComponent implements OnInit {

  @Input() informeFinalId : number;
  ELEMENT_DATA: any[] = [];
  displayedColumns: string[] = [
    'numero',
    'item',
    'tipoAnexoString',
    'ubicacion'
  ];
  dataSource = new MatTableDataSource(this.ELEMENT_DATA);
  datosTabla = [];

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private registerContractualLiquidationRequestService: RegisterContractualLiquidationRequestService) { }

  ngOnInit(): void {
    this.getInformeFinalAnexoByInformeFinalId(this.informeFinalId);
  }

  getInformeFinalAnexoByInformeFinalId(informeFinalId: number) {
    this.registerContractualLiquidationRequestService.getInformeFinalAnexoByInformeFinalId(informeFinalId).subscribe(report => {
      if(report != null){
        report.forEach(element => {
          this.datosTabla.push({
            numero: element.informeFinalListaChequeo.posicion,
            item: element.informeFinalListaChequeo.nombre,
            tipoAnexoString: element.informeFinalAnexo ? element.informeFinalAnexo.tipoAnexoString : "",
            calificacionCodigo: element.calificacionCodigo,
            numRadicadoSac: element.informeFinalAnexo ? element.informeFinalAnexo.numRadicadoSac : "",
            fechaRadicado:element.informeFinalAnexo ? element.informeFinalAnexo.fechaRadicado : "",
            urlSoporte:  element.informeFinalAnexo ?element.informeFinalAnexo.urlSoporte : "",
            tipoAnexo: element.informeFinalAnexo ? element.informeFinalAnexo.tipoAnexo : ""
          });
        })
      }
      this.dataSource.data = this.datosTabla;
    });
  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.nextPageLabel = 'Siguiente';
    this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
      if (length === 0 || pageSize === 0) {
        return '0 de ' + length;
      }
      length = Math.max(length, 0);
      const startIndex = page * pageSize;
      // If the start index exceeds the list length, do not try and fix the end index to the end.
      const endIndex = startIndex < length ?
        Math.min(startIndex + pageSize, length) :
        startIndex + pageSize;
      return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
    };
    this.paginator._intl.previousPageLabel = 'Anterior';
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

}