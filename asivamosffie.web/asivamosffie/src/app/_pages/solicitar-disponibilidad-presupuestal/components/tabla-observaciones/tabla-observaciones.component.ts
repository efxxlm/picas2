import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core'
import { MatPaginator } from '@angular/material/paginator'
import { MatSort } from '@angular/material/sort'
import { MatTableDataSource } from '@angular/material/table'

const ELEMENT_DATA: any[] = [
  {
    fecha: '21/06/2020',
    autor: 'Johan Galeano',
    observacion: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla imperdiet vestibulum sem, vel ultrices felis vulputate vel.'
  }
]

@Component({
  selector: 'app-tabla-observaciones',
  templateUrl: './tabla-observaciones.component.html',
  styleUrls: ['./tabla-observaciones.component.scss']
})
export class TablaObservacionesComponent implements OnInit, AfterViewInit {
  displayedColumns: string[] = ['fecha', 'autor', 'observacion']
  dataSource = new MatTableDataSource(ELEMENT_DATA)
  @ViewChild(MatPaginator) paginator: MatPaginator
  @ViewChild(MatSort) sort: MatSort

  constructor() {}

  ngOnInit(): void {}

  ngAfterViewInit() {
    this.dataSource.sort = this.sort
    this.dataSource.paginator = this.paginator
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página'
    this.paginator._intl.nextPageLabel = 'Siguiente'
    this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
      return (page + 1).toString() + ' de ' + length.toString()
    }
    this.paginator._intl.previousPageLabel = 'Anterior'
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value
    this.dataSource.filter = filterValue.trim().toLowerCase()
    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage()
    }
  }
}
