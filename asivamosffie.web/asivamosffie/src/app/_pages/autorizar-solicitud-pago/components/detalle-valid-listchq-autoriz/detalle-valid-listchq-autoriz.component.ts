import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-detalle-valid-listchq-autoriz',
  templateUrl: './detalle-valid-listchq-autoriz.component.html',
  styleUrls: ['./detalle-valid-listchq-autoriz.component.scss']
})
export class DetalleValidListchqAutorizComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'item',
    'documento',
    'revTecnica'
  ];
  dataTable: any[] = [
    {
      item: 1,
      documento: 'Certificación de supervisión de aprobación de pago de interventoría suscrita por el contratista y el Supervisor en original, en el cual se evidencie el avance porcentual de obra.',
      revTecnica: 'Sí cumple'
    },
    {
      item: 2,
      documento: 'Factura (Prefactura si tiene facturación electrónica) por costo variable del Contrato de Interventoría, cuyo deudor es el Patrimonio Autónomo del Fondo de Infraestructura Educativa FFIE, NIT 830.053.812-2. Indicar el lugar en el que se presta el servicio. Para la instrucción de pago se sugiere la siguiente nota: “Favor realizar transferencia electrónica (consignación) a la cuenta (mencionar el tipo: ahorro o corriente) No. XXXXX en el (nombre de banco), a nombre de (XXXXXXXXX) con NIT (XXXXXXXX).',
      revTecnica: 'Sí cumple'
    },
    {
      item: 3,
      documento: 'Copia de la resolución de facturación vigente.',
      revTecnica: 'Sí cumple'
    },
    {
      item: 4,
      documento: 'Copia del Acta de Inicio de la Fase del Contrato de Interventoría (Requerido para el primer pago).',
      revTecnica: 'Sí cumple'
    },
    {
      item: 5,
      documento: 'Acta parcial de Interventoría suscrita por el contratista y el Supervisor, de forma impresa y con una copia magnética en formato Excel editable.',
      revTecnica: 'Sí cumple'
    },
    {
      item: 6,
      documento: 'Certificación revisor fiscal con copia de la tarjeta profesional y/o representante legal (únicamente cuando la sociedad no esté obligada a tener revisor fiscal) en la cual certifique que están al día en el pago de nómina, seguridad social y parafiscales del consorcio y los consorciados (debe corresponder al periodo en el que se emite la factura. Si la factura tiene fecha de los cinco (5) primeros días hábiles del mes, se podrá anexar la certificación del mes anterior).',
      revTecnica: 'Sí cumple'
    },
    {
      item: 7,
      documento: 'Certificación bancaria original no mayor a treinta (30) días, la cual debe contener los siguientes datos: Número de cuenta, clase de cuenta, NIT, nombre del titular y banco al cual se le debe realizar el pago (para el primer pago y/o cada vez que se cambie).',
      revTecnica: 'Sí cumple'
    },
    {
      item: 8,
      documento: 'RUT del Contratista. En el caso de un Consorcio o Unión Temporal, se deberá aportar el RUT de la forma plural de asociación y de cada uno de los integrantes (para el primer pago y/o cada vez que se actualice).',
      revTecnica: 'Sí cumple'
    },
    {
      item: 9,
      documento: 'Copia documento de constitución del consorcio o unión temporal.',
      revTecnica: 'Sí cumple'
    },
    {
      item: 10,
      documento: 'Formato de retención en la fuente (si es persona natural) (Requerido para el primer pago).',
      revTecnica: 'Sí cumple'
    }
  ];
  constructor() { }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  };

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };


}
