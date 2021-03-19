import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-detalle-informe-final-gtlc',
  templateUrl: './detalle-informe-final-gtlc.component.html',
  styleUrls: ['./detalle-informe-final-gtlc.component.scss']
})
export class DetalleInformeFinalGtlcComponent implements OnInit {
  //Tabla informe final y anexos
  dataSource = new MatTableDataSource();
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  displayedColumns: string[] = ['numero','item','tipoAnexo','ubicacion'];
  datatable: any [] = [
    {
      id : 1,
      item: 'Acta de recibo a satisfacción de la fase 2 - Interventoría',
      tipoAnexo : 'Digital',
      ubicacion: '<em>https://drive.google.com/drive/actadereciboasatisfaccion</em>'
    },
    {
      id : 2,
      item: 'Acta de Inventario por espacios',
      tipoAnexo : 'Físico',
      ubicacion: '<b>SAC: </b>90877654<br><b>Fecha: </b>20/11/2020'
    },
    {
      id : 3,
      item: 'Informe final de Obra del contratista de obra',
      tipoAnexo : 'Físico',
      ubicacion: '<b>SAC: </b>90878765<br><b>Fecha: </b>10/11/2020'
    },
    {
      id : 4,
      item: 'Memoria descriptiva del proyecto',
      tipoAnexo : '',
      ubicacion: ''
    },
    {
      id : 5,
      item: 'Licencia de construcción, modificaciones y demás permiso requeridos',
      tipoAnexo : '',
      ubicacion: ''
    },
    {
      id : 6,
      item: 'Acta de recibo bomberos',
      tipoAnexo : 'Digital',
      ubicacion: '<em>https://drive.google.com/drive/actaderecibobomberos</em>'
    },
    {
      id : 7,
      item: 'Planos Récord arquitectónicos',
      tipoAnexo : 'Físico',
      ubicacion: '<b>SAC: </b>90898754<br><b>Fecha: </b>15/11/2020'
    },
    {
      id : 8,
      item: 'Planos Récord estructurales',
      tipoAnexo : 'Físico',
      ubicacion: '<b>SAC: </b>90825674<br><b>Fecha: </b>13/11/2020'
    },
    {
      id : 9,
      item: 'Planos Récord hidrosanitarios y gas',
      tipoAnexo : 'Físico',
      ubicacion: '<b>SAC: </b>90809752<br><b>Fecha: </b>29/10/2020'
    },    
    {
      id : 10,
      item: 'Planos Récord eléctricos',
      tipoAnexo : 'Físico',
      ubicacion: '<b>SAC: </b>90876543<br><b>Fecha: </b>20/10/2020'
    },
    {
      id : 11,
      item: 'Actas de socialización de esquema básico - Fase 1',
      tipoAnexo : '',
      ubicacion: ''
    },
  ];
  constructor() { }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource(this.datatable);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
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
  }

}
