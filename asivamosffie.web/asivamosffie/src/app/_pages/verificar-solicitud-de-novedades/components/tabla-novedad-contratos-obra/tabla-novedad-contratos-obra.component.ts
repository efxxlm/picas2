import { Component, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ContractualNoveltyService } from 'src/app/core/_services/ContractualNovelty/contractual-novelty.service';

@Component({
  selector: 'app-tabla-novedad-contratos-obra',
  templateUrl: './tabla-novedad-contratos-obra.component.html',
  styleUrls: ['./tabla-novedad-contratos-obra.component.scss']
})
export class TablaNovedadContratosObraComponent implements AfterViewInit {

  displayedColumns: string[] = [
    'fecha',
    'numeroSolicitud',
    'numeroContrato',
    'tipoNovedad',
    'estadoNovedad',
    'estadoRegistro',
    'id'
  ];
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private contractualNoveltyService: ContractualNoveltyService,

  ) { }

  ngAfterViewInit() {
    this.contractualNoveltyService.getListGrillaNovedadContractualObra()
      .subscribe(resp => {
        this.dataSource = new MatTableDataSource(resp);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.nextPageLabel = 'Siguiente';
    this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
      return (page + 1).toString() + ' de ' + length.toString();
    };
    this.paginator._intl.previousPageLabel = 'Anterior';
  });
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

}
