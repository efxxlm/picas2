import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';

@Component({
  selector: 'app-validar-balance-gbftrec',
  templateUrl: './validar-balance-gbftrec.component.html',
  styleUrls: ['./validar-balance-gbftrec.component.scss']
})
export class ValidarBalanceGbftrecComponent implements OnInit {
  id: string;
  opcion1 = false;
  opcion2 = false;
  opcion3 = false;
  constructor(
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.id = params.id;
    });
  }

}
